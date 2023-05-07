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
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectController(ApplicationDbContext applicationDbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await applicationDbContext.Projects.ToListAsync();
            var models = new List<ProjectIndexViewModel>();
            var missions = await applicationDbContext.Missions.ToListAsync();
            foreach (var project in projects)
            {
                var model = new ProjectIndexViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Deadline = project.Deadline,
                    CreatedDate = project.CreateDate,
                };
                var curMissions = missions.Where(a => a.MissionProjectId == project.Id).ToList();
                var curMissionsCount = curMissions.Count() == 0 ? 1 : curMissions.Count();
                var unDealMissionCount = curMissions.Where(a => a.Status == MissionStatus.待处理 && a.Deadline >= DateTime.Now).Count();
                var compeleteMissionCount = curMissions.Where(a => a.Status == MissionStatus.进行中 && a.Deadline >= DateTime.Now).Count();
                var inprogressMissionCount = curMissions.Where(a => a.Status == MissionStatus.已完成 && a.Deadline >= DateTime.Now).Count();
                var timeoutMissionCount = curMissions.Where(a => a.Deadline < DateTime.Now).Count();
                model.UnDealPercent = unDealMissionCount / curMissionsCount;
                model.FinishedPercent = compeleteMissionCount / curMissionsCount;
                model.InProgressPercent = inprogressMissionCount / curMissionsCount;
                model.TimeOutPercent = timeoutMissionCount / curMissionsCount;
                models.Add(model);
            }

            return View(models);
        }
        public async Task<IActionResult> ProjectDetail(int id)
        {
            var project = await applicationDbContext.Projects.Include(a => a.Risks).FirstOrDefaultAsync(a => a.Id == id);
            var missions = await applicationDbContext.Missions.Include(a => a.Executors).Where(a => a.MissionProjectId == project.Id).ToListAsync();
            var model = new ProjectDetailViewModel()
            {
                ProjectEditViewModel = new ProjectEditViewModel()
                {
                    Id = project.Id,
                    Name = project.Name,
                    Deadline = project.Deadline,
                    Description = project.Description,
                    CreatedDate = project.CreateDate,
                    Functionary = (await applicationDbContext.Users.FirstOrDefaultAsync(a => a.Id == project.FunctionaryId)).UserName,
                    PutForward = (await applicationDbContext.Users.FirstOrDefaultAsync(a => a.Id == project.PutForwardId)).UserName,
                },

                ProjectMissionIndexViewModel = new ProjectMissionIndexViewModel()
                {
                    FinishedMissions = missions.Where(a => a.Status == MissionStatus.已完成)
                                        .Select(a => new ProjectEditMissionViewModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Description = a.Description,
                                            Deadline = a.Deadline,
                                            CreateDate = a.CreateDate,
                                            Priority = a.Priority,
                                            Status = a.Status,
                                            Executors = a.Executors.Select(a => a.UserName).ToList(),
                                        }),
                    UntreatedMissions = missions.Where(a => a.Status == MissionStatus.待处理)
                                        .Select(a => new ProjectEditMissionViewModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Description = a.Description,
                                            Deadline = a.Deadline,
                                            CreateDate = a.CreateDate,
                                            Priority = a.Priority,
                                            Status = a.Status,
                                            Executors = a.Executors.Select(a => a.UserName).ToList(),
                                        }),
                    ProcessOnMissions = missions.Where(a => a.Status == MissionStatus.进行中)
                                        .Select(a => new ProjectEditMissionViewModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Description = a.Description,
                                            Deadline = a.Deadline,
                                            CreateDate = a.CreateDate,
                                            Priority = a.Priority,
                                            Status = a.Status,
                                            Executors = a.Executors.Select(a => a.UserName).ToList(),
                                        }),
                },
                CurProject = project,
                AddMission = new ProjectAddMissionViewModel
                {
                    Deadline = DateTime.Now,
                },
                EditMission = new ProjectEditMissionViewModel { },
            };
            model.RiskEditViewModels = project.Risks.Select(r => new RiskEditViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Incidence = r.Incidence,
                Solution = r.Solution,
                CreateDate = r.CreateDate,
                Status = r.Status,
                RiskType = r.RiskType,
                Level = r.Level,
                Functionary = r.Functionary,
                FunctionaryId = r.FunctionaryId,
                PutForward = r.PutForward,
                PutForwardId = r.PutForwardId,
                Project = project,
                ProjectId = project.Id,
            });

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProject([Bind("ProjectEditViewModel")] ProjectDetailViewModel model)
        {
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.ProjectEditViewModel.Id);
            project.Name = model.ProjectEditViewModel.Name;
            project.Description = model.ProjectEditViewModel.Description;
            project.CreateDate = model.ProjectEditViewModel.CreatedDate;
            project.UpdateDate = DateTime.Now;
            project.Deadline = model.ProjectEditViewModel.Deadline;
            project.Budget = model.ProjectEditViewModel.Budget;
            project.FunctionaryId = (await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.ProjectEditViewModel.Functionary)).Id;
            project.PutForwardId = (await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.ProjectEditViewModel.PutForward)).Id;
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = project.Id, tab = "bordered-editProject" });
        }

        public IActionResult AddProject()
        {
            var model = new ProjectAddViewModel()
            {
                PutForward = User.Identity.Name,
                Deadline = DateTime.Now,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectAddViewModel model)
        {
            var projectModel = new Project
            {
                Name = model.Name,
                Description = model.Description,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                FunctionaryId = (await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.Functionary)).Id,
                PutForwardId = (await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.PutForward)).Id,
                Status = (ProjectStatus)model.Status,
            };
            await applicationDbContext.Projects.AddAsync(projectModel);
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

        public async Task<IActionResult> AddMission([Bind("AddMission")] ProjectDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            var excutors = model.AddMission.Executors;
            var curMission = model.AddMission;
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Name == model.AddMission.ProjectName);
            var mission = new Mission()
            {
                Name = curMission.Name,
                Description = curMission.Description,
                Deadline = curMission.Deadline,
                CreateDate = DateTime.Now,
                Priority = curMission.Priority,
                Status = curMission.Status,
                MissionProjectId = project.Id,
                PutForwardId = (await userManager.FindByNameAsync(User.Identity.Name)).Id,
                Executors = new List<ApplicationUser>(),
            };
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var user in excutors)
            {
                users.Add(await applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == user));
            }
            mission.Executors.AddRange(users);
            await applicationDbContext.Missions.AddAsync(mission);
            applicationDbContext.SaveChanges(true);
            return RedirectToAction("ProjectDetail", new { id = project.Id });
        }

        public async Task<IActionResult> EditMission([Bind("EditMission")] ProjectDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            var excutors = model.EditMission.Executors;
            var curMission = model.EditMission;
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Name == model.EditMission.ProjectName);
            var mission = await applicationDbContext.Missions.Include(a => a.Executors).FirstOrDefaultAsync(a => a.Id == model.EditMission.Id);

            mission.Name = curMission.Name;
            mission.Description = curMission.Description;
            mission.Deadline = curMission.Deadline;
            mission.CreateDate = DateTime.Now;
            mission.Priority = curMission.Priority;
            mission.Status = curMission.Status;
            mission.MissionProjectId = project.Id;
            mission.Executors.Clear();

            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var user in excutors)
            {
                users.Add(await applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == user));
            }
            mission.Executors.AddRange(users);
            applicationDbContext.Missions.Update(mission);
            applicationDbContext.SaveChanges(true);
            return RedirectToAction("ProjectDetail", new { id = project.Id });
        }

        public async Task<IActionResult> DeleteMission(int id)
        {
            var missionId = id;
            var mission = await applicationDbContext.Missions.FirstOrDefaultAsync(c => c.Id == missionId);
            var projectId = mission.MissionProjectId;
            applicationDbContext.Missions.Remove(mission);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", new { id = projectId });
        }
    }
}
