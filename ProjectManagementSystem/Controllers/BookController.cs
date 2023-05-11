using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Net.Mime;

namespace ProjectManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IWebHostEnvironment env;

        public BookController(ApplicationDbContext applicationDbContext, IWebHostEnvironment env)
        {
            this.applicationDbContext = applicationDbContext;
            this.env = env;
        }

        public IActionResult AddBook(int projectId)
        {
            var book = new BookAddViewModel
            {
                ProjectId = projectId,
            };
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookAddViewModel model)
        {
            var book = new Book
            {
                Name = model.Name,
                Content = model.Content,
                Summary = model.Summary,
                CoverImage = SaveCoverImage(model.CoverImage),
                Project = await applicationDbContext.Projects.FindAsync(model.ProjectId),
            };
            await applicationDbContext.Books.AddAsync(book);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = book.Project.Id, tab = "bordered-books" });
        }

        public async Task<IActionResult> EditBook(int id)
        {
            var book = await applicationDbContext.Books.Include(b => b.Project).FirstOrDefaultAsync(b => b.Id == id);
            var model = new BookEditViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Content = book.Content,
                Summary = book.Summary,
                CoverImagePath = book.CoverImage,
                ProjectId = book.Project.Id,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(BookEditViewModel model)
        {
            var book = await applicationDbContext.Books.FindAsync(model.Id);
            book.Name = model.Name;
            book.Content = model.Content;
            book.Summary = model.Summary;
            if (model.CoverImage != null)
            {
                book.CoverImage = SaveCoverImage(model.CoverImage);
            }
            book.Project = await applicationDbContext.Projects.FindAsync(model.ProjectId);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = book.Project.Id, tab = "bordered-books" });
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await applicationDbContext.Books.Include(b => b.Project).FirstOrDefaultAsync(b => b.Id == id);
            applicationDbContext.Books.Remove(book);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = book.Project.Id, tab = "bordered-books" });
        }

        private string SaveCoverImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(env.WebRootPath, "document", fileName);

                // 检查文件是否已经存在
                if (System.IO.File.Exists(filePath))
                {
                    return $"/document/{fileName}";
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return $"/document/{fileName}";
            }
            else
            {
                return null;
            }
        }

        public IActionResult ReturnProjetDetail(int id)
        {
            return RedirectToAction("ProjectDetail", "Project", new { id = id, tab = "bordered-books" });
        }

    }
}
