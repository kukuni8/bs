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
                Summary = model.Summary,
                BookFile = SaveFile(model.BookFile),
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
                Summary = book.Summary,
                FilePath = book.BookFile,
                ProjectId = book.Project.Id,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditBook(BookEditViewModel model)
        {
            var book = await applicationDbContext.Books.FindAsync(model.Id);
            book.Name = model.Name;
            book.Summary = model.Summary;
            if (model.BookFile != null)
            {
                book.BookFile = SaveFile(model.BookFile);
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

        public async Task<IActionResult> DownloadBook(string fileName)
        {
            if (fileName == null)
                return Content("filename not present");

            var path = Path.Combine(env.WebRootPath, fileName.TrimStart('/'));



            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            // 猜测MIME类型
            var mime = GetMimeType(fileName);

            return File(memory, mime, Path.GetFileName(path));
        }


        private string SaveFile(IFormFile file)
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

        public static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            switch (extension)
            {
                case ".txt": return "text/plain";
                case ".pdf": return "application/pdf";
                case ".doc": return "application/vnd.ms-word";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".gif": return "image/gif";
                case ".csv": return "text/csv";
                default: return "application/octet-stream";  // for unknown types
            }
        }


    }
}
