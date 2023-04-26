using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ProjectController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await applicationDbContext.Projects.ToListAsync();
            var model = new ProjectIndexViewModal
            {
                projects = projects,
                //HelpProject = new Project(),
            };
            return View(model);
        }
        public async Task<IActionResult> EditProject(int id)
        {
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == id);
            var model = new ProjectEditViewModel()
            {
                Project = project,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProject(ProjectEditViewModel model)
        {
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.Project.Id);
            project.Name = model.Project.Name;
            project.Description = model.Project.Description;
            project.CreatedDate = model.Project.CreatedDate;
            project.UpdatedDate = model.Project.UpdatedDate;
            project.Deadline = model.Project.Deadline;
            project.Status = model.Project.Status;
            project.Budget = model.Project.Budget;
            project.Functionary = model.Project.Functionary;
            project.PutForward = model.Project.PutForward;
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectAddViewModel model)
        {
            await applicationDbContext.Projects.AddAsync(model.Project);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(b => b.Id == id);
            applicationDbContext.Remove(project);
            applicationDbContext.SaveChanges(true);
            return RedirectToAction("Index");
        }
        public IActionResult AddTable()
        {
            applicationDbContext.ProjectStatuses.Add(new ProjectStatus
            {
                StatusName = "待处理",
            });
            applicationDbContext.ProjectStatuses.Add(new ProjectStatus
            {
                StatusName = "进行中",
            });
            applicationDbContext.ProjectStatuses.Add(new ProjectStatus { StatusName = "已完成" });
            applicationDbContext.SaveChanges();
            return View();
        }
    }
}
