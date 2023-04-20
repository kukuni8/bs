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
        private readonly DataDbContext dataDbContext;

        public ProjectController(DataDbContext dataDbContext)
        {
            this.dataDbContext = dataDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await dataDbContext.Projects.ToListAsync();
            return View(projects);
        }
        public async Task<IActionResult> EditProject(int id)
        {
            var project = await dataDbContext.Projects.FirstOrDefaultAsync(a => a.Id == id);
            var model = new ProjectEditViewModel()
            {
                Project = project,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProject(ProjectEditViewModel model)
        {
            var project = await dataDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.Project.Id);
            project.Name = model.Project.Name;
            project.Description = model.Project.Description;
            project.CreatedDate = model.Project.CreatedDate;
            project.UpdatedDate = model.Project.UpdatedDate;
            project.Deadline = model.Project.Deadline;
            project.Status = model.Project.Status;
            project.Budget = model.Project.Budget;
            project.Functionary = model.Project.Functionary;
            project.PutForward = model.Project.PutForward;
            dataDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectAddViewModel model)
        {
            await dataDbContext.Projects.AddAsync(model.Project);
            dataDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await dataDbContext.Projects.FirstOrDefaultAsync(b => b.Id == id);
            dataDbContext.Remove(project);
            dataDbContext.SaveChanges(true);
            return RedirectToAction("Index");
        }
        public IActionResult AddTable()
        {
            dataDbContext.ProjectStatuses.Add(new ProjectStatus
            {
                StatusName = "待处理",
            });
            dataDbContext.ProjectStatuses.Add(new ProjectStatus
            {
                StatusName = "进行中",
            });
            dataDbContext.ProjectStatuses.Add(new ProjectStatus { StatusName = "已完成" });
            dataDbContext.SaveChanges();
            return View();
        }
    }
}
