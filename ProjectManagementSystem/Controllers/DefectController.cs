using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class DefectController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public DefectController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var defects = await applicationDbContext.Defects.ToListAsync();
            return View(defects);
        }
        [Authorize(Policy = "缺陷添加")]
        public async Task<IActionResult> AddDefect(int projectId)
        {
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == projectId);
            var model = new DefectAddViewModel
            {
                PutForward = await userManager.FindByNameAsync(User.Identity.Name),
                PutForwardId = (await userManager.FindByNameAsync(User.Identity.Name)).Id,
                ProjectId = projectId,
                Project = project,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddDefect(DefectAddViewModel model)
        {
            var project = await applicationDbContext.Projects.Include(p => p.Defects).FirstOrDefaultAsync(a => a.Id == model.ProjectId);
            var defect = new Defect
            {
                Name = model.Name,
                Description = model.Description,
                CreateDate = DateTime.Now,
                Solution = model.Solution,
                Type = model.Type,
                Status = model.Status,
                PutForwardId = model.PutForwardId,
                PutForward = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == model.PutForwardId),
                Project = project,
            };
            project.Defects.Add(defect);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = model.ProjectId, tab = "bordered-defects" });
        }
        [Authorize(Policy = "缺陷编辑")]
        public async Task<IActionResult> EditDefect(int Id)
        {
            var defect = await applicationDbContext.Defects.Include(d => d.Project).Include(d => d.PutForward).FirstOrDefaultAsync(x => x.Id == Id);
            var model = new DefectEditViewModel
            {
                Id = Id,
                Name = defect.Name,
                Description = defect.Description,
                CreateDate = defect.CreateDate,
                Solution = defect.Solution,
                Type = defect.Type,
                Status = defect.Status,
                PutForward = defect.PutForward,
                PutForwardId = defect.PutForwardId,
                ProjectId = defect.Project.Id,
                Functionary = await userManager.FindByNameAsync(User.Identity.Name),
                FunctionaryId = (await userManager.FindByNameAsync(User.Identity.Name)).Id,
                Project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == defect.Project.Id),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDefect(DefectEditViewModel model)
        {
            var defect = await applicationDbContext.Defects.Include(d => d.Project).FirstOrDefaultAsync(a => a.Id == model.Id);
            defect.Name = model.Name;
            defect.Description = model.Description;
            defect.CreateDate = model.CreateDate;
            defect.Solution = model.Solution;
            defect.Type = model.Type;
            defect.Status = model.Status;
            defect.PutForward = model.PutForward;
            defect.PutForwardId = model.PutForwardId;
            defect.Project = defect.Project;
            defect.FunctionaryId = model.FunctionaryId;
            defect.Functionary = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == defect.FunctionaryId);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = defect.Project.Id, tab = "bordered-defects" });
        }
        [Authorize(Policy = "缺陷删除")]
        public async Task<IActionResult> DeleteDefect(int Id)
        {
            var defect = await applicationDbContext.Defects.Include(d => d.Project).FirstOrDefaultAsync(a => a.Id == Id);
            applicationDbContext.Defects.Remove(defect);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = defect.Project.Id, tab = "bordered-defects" });
        }
        public IActionResult ReturnDetail(int projectId)
        {
            return RedirectToAction("ProjectDetail", "Project", new { id = projectId, tab = "bordered-defects" });
        }
    }
}
