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
            var models = new List<ProjectIndexViewModal>();
            var missions = await applicationDbContext.Missions.Include(a => a.Status).ToListAsync();
            foreach (var project in projects)
            {
                var model = new ProjectIndexViewModal
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Deadline = project.Deadline,
                    CreateTime = project.CreatedDate,
                };
                var curMissions = missions.Where(a => a.ProjectId == project.Id).ToList();
                var curMissionsCount = curMissions.Count() == 0 ? 0f : 1f;
                var unDealMissionCount = curMissions.Where(a => a.Status.Status == "待处理" && a.Deadline >= DateTime.Now).Count();
                var compeleteMissionCount = curMissions.Where(a => a.Status.Status == "已完成" && a.Deadline >= DateTime.Now).Count();
                var inprogressMissionCount = curMissions.Where(a => a.Status.Status == "进行中" && a.Deadline >= DateTime.Now).Count();
                var timeoutMissionCount = curMissions.Where(a => a.Deadline < DateTime.Now).Count();
                model.UnDealPercent = unDealMissionCount / curMissionsCount;
                model.FinishedPercent = compeleteMissionCount / curMissionsCount;
                model.InProgressPercent = inprogressMissionCount / curMissionsCount;
                model.TimeOutPercent = timeoutMissionCount / curMissionsCount;
                models.Add(model);
            }

            return View(models);
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
