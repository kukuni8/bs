using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class ResourceController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<ApplicationUser> userManager;

        public ResourceController(ApplicationDbContext applicationDbContext, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.env = env;
            this.userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> InitResource([Bind("ResourceAddViewModel")] ProjectDetailViewModel vm)
        {
            var model = vm.ResourceAddViewModel;
            var res = new Resource()
            {
                Name = model.Name,
                Description = model.Description,
                Number = model.Number,
                ImagePath = SaveImage(model.Image),
                Project = await applicationDbContext.Projects.FindAsync(model.ProjectId),
            };
            var resChange = new ResourceChange()
            {
                Number = model.Number,
                Resource = res,
                Description = "初始化",
                Time = DateTime.Now,
                User = await userManager.FindByNameAsync(User.Identity.Name),
                Reason = "初始化",
            };
            await applicationDbContext.Resources.AddAsync(res);
            await applicationDbContext.SaveChangesAsync();
            await applicationDbContext.ResourceChanges.AddAsync(resChange);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = model.ProjectId, tab = "bordered-resources" });
        }

        [HttpPost]
        public async Task<IActionResult> UseResource([Bind("UseResourceViewModel")] ProjectDetailViewModel vm)
        {
            var model = vm.UseResourceViewModel;
            var resource = await applicationDbContext.Resources
                .Include(r => r.Changes)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(r => r.Id == model.ResourceId);
            var resChange = new ResourceChange()
            {
                Resource = resource,
                Description = "使用",
                Time = DateTime.Now,
                User = await userManager.FindByNameAsync(User.Identity.Name),
                Reason = model.Description,
                Number = model.Number,
            };
            resource.Number = resource.Number - model.Number;
            applicationDbContext.Resources.Update(resource);
            await applicationDbContext.ResourceChanges.AddAsync(resChange);
            await applicationDbContext.SaveChangesAsync(true);
            return RedirectToAction("ProjectDetail", "Project", new { id = model.ProjectId, tab = "bordered-resources" });
        }

        [HttpPost]
        public async Task<IActionResult> AddResource([Bind("AddResourceViewModel")] ProjectDetailViewModel vm)
        {
            var model = vm.AddResourceViewModel;
            var resource = await applicationDbContext.Resources
                .Include(r => r.Changes)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(r => r.Id == model.ResourceId);
            var resChange = new ResourceChange()
            {
                Resource = resource,
                Description = "添加",
                Time = DateTime.Now,
                User = await userManager.FindByNameAsync(User.Identity.Name),
                Reason = model.Description,
                Number = model.Number,
            };
            resource.Number = resource.Number + model.Number;
            applicationDbContext.Resources.Update(resource);
            await applicationDbContext.ResourceChanges.AddAsync(resChange);
            await applicationDbContext.SaveChangesAsync(true);
            return RedirectToAction("ProjectDetail", "Project", new { id = model.ProjectId, tab = "bordered-resources" });
        }

        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await applicationDbContext.Resources
                .Include(r => r.Project)
                .Include(r => r.Changes)
                .FirstOrDefaultAsync(r => r.Id == id);
            var changes = resource.Changes;
            applicationDbContext.ResourceChanges.RemoveRange(changes);
            applicationDbContext.Resources.Remove(resource);
            await applicationDbContext.SaveChangesAsync(true);
            return RedirectToAction("ProjectDetail", "Project", new { id = resource.Project.Id, tab = "bordered-resources" });
        }


        private string SaveImage(IFormFile file)
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
    }
}
