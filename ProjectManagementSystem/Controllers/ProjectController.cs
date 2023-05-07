using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Reflection;

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
            var projects = await applicationDbContext.Projects.Include(a => a.Missions).ToListAsync();
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
                var curMissions = project.Missions.ToList();
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
            var project = await applicationDbContext.Projects
                .Include(a => a.Risks)
                .ThenInclude(r => r.PutForward)
                .Include(a => a.Risks)
                .ThenInclude(r => r.Functionary)
                .Include(p => p.Defects)
                .Include(p => p.Missions)
                .ThenInclude(m => m.MissionExecutors)
                .ThenInclude(me => me.ApplicationUser)
                .Include(p => p.ProjectUsers)
                .FirstOrDefaultAsync(a => a.Id == id);
            var missions = project.Missions;
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
                                            StartDate = DateTime.Now,
                                            FinishedTime = DateTime.Now,
                                            Priority = a.Priority,
                                            Status = a.Status,
                                            Executors = a.MissionExecutors.Select(a => a.ApplicationUser.UserName).ToList(),
                                        }),
                    UntreatedMissions = missions.Where(a => a.Status == MissionStatus.待处理)
                                        .Select(a => new ProjectEditMissionViewModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Description = a.Description,
                                            Deadline = a.Deadline,
                                            CreateDate = a.CreateDate,
                                            StartDate = DateTime.Now,
                                            FinishedTime = DateTime.Now,
                                            Priority = a.Priority,
                                            Status = a.Status,
                                            Executors = a.MissionExecutors.Select(a => a.ApplicationUser.UserName).ToList(),
                                        }),
                    ProcessOnMissions = missions.Where(a => a.Status == MissionStatus.进行中)
                                        .Select(a => new ProjectEditMissionViewModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Description = a.Description,
                                            Deadline = a.Deadline,
                                            CreateDate = a.CreateDate,
                                            StartDate = DateTime.Now,
                                            FinishedTime = DateTime.Now,
                                            Priority = a.Priority,
                                            Status = a.Status,
                                            Executors = a.MissionExecutors.Select(a => a.ApplicationUser.UserName).ToList(),
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

            model.DefectEditViewModels = project.Defects.Select(d => new DefectEditViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreateDate = d.CreateDate,
                Status = d.Status,
                Solution = d.Solution,
                Type = d.Type,
                Functionary = d.Functionary,
                FunctionaryId = d.FunctionaryId,
                PutForward = d.PutForward,
                PutForwardId = d.PutForwardId,
                Project = project,
                ProjectId = project.Id,
            });
            model.ProjectUserIndexViewModels = project.ProjectUsers.Select(u => new ProjectUserIndexViewModel
            {
                Id = u.ApplicationUser.Id,
                Name = u.ApplicationUser.UserName,
                Department = u.ApplicationUser.Department,
                Job = u.ApplicationUser.Job,
                RoleName = u.ApplicationUser.RoleName,
            });
            model.UsersNotInThisProject = await applicationDbContext.Users.Where(u => !project.ProjectUsers.Select(a => a.ApplicationUserId).Contains(u.Id)).Select(u => new ProjectUserNotInProjectModel
            {
                Id = u.Id,
                Name = u.UserName,
                Department = u.Department,
                Job = u.Job,
                RoleName = u.RoleName,
                IsSelected = false,
            }).ToListAsync();
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
            var func = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.Functionary);
            var put = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.PutForward);
            var projectModel = new Project
            {
                Name = model.Name,
                Description = model.Description,
                CreateDate = DateTime.Now,
                Deadline = DateTime.Now,
                FunctionaryId = func.Id,
                PutForwardId = put.Id,
                Status = (ProjectStatus)model.Status,
                ProjectUsers = new List<ProjectUser>(),
            };
            var pu = new ProjectUser
            {
                Project = projectModel,
                ApplicationUser = put,
            };
            projectModel.ProjectUsers.Add(pu);
            if (func.Id != put.Id)
            {
                var pu1 = new ProjectUser
                {
                    Project = projectModel,
                    ApplicationUser = func,
                };
                projectModel.ProjectUsers.Add(pu);
            }
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
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == curMission.ProjectId);
            var mission = new Mission()
            {
                Name = curMission.Name,
                Description = curMission.Description,
                Deadline = curMission.Deadline,
                CreateDate = DateTime.Now,
                StartDate = curMission.StartDate,
                FinishedTime = curMission.FinishedTime,
                Priority = curMission.Priority,
                Status = curMission.Status,
                Project = project,
                PutForwardId = (await userManager.FindByNameAsync(User.Identity.Name)).Id,
                MissionExecutors = new List<MissionExecutor>(),
            };
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var user in excutors)
            {
                var me = new MissionExecutor
                {
                    ApplicationUser = await applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == user),
                    Mission = mission,
                };
                mission.MissionExecutors.Add(me);
            }

            await applicationDbContext.Missions.AddAsync(mission);
            applicationDbContext.SaveChanges(true);
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-missions" });
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
            var mission = await applicationDbContext.Missions.Include(a => a.MissionExecutors).FirstOrDefaultAsync(a => a.Id == model.EditMission.Id);

            mission.Name = curMission.Name;
            mission.Description = curMission.Description;
            mission.Deadline = curMission.Deadline;
            mission.CreateDate = curMission.CreateDate;
            mission.StartDate = DateTime.Now;
            mission.FinishedTime = DateTime.Now;
            mission.Deadline = curMission.Deadline;
            mission.Priority = curMission.Priority;
            mission.Status = curMission.Status;
            mission.Project.Id = project.Id;
            mission.MissionExecutors.Clear();

            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var user in excutors)
            {
                var me = new MissionExecutor
                {
                    ApplicationUser = await applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == user),
                    Mission = mission,
                };
                mission.MissionExecutors.Add(me);
            }
            applicationDbContext.Missions.Update(mission);
            applicationDbContext.SaveChanges(true);
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-missions" });
        }

        public async Task<IActionResult> DeleteMission(int id)
        {
            var missionId = id;
            var mission = await applicationDbContext.Missions.Include(m => m.Project).FirstOrDefaultAsync(c => c.Id == missionId);
            var projectId = mission.Project.Id;
            applicationDbContext.Missions.Remove(mission);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-missions" });
        }

        public async Task<IActionResult> AddUserToProject([Bind("UsersNotInThisProject,CurProjectId")] ProjectDetailViewModel model)
        {
            var users = model.UsersNotInThisProject.Where(u => u.IsSelected);
            foreach (var user in users)
            {
                var pu = new ProjectUser
                {
                    ProjectId = model.CurProjectId,
                    ApplicationUserId = user.Id,
                };
                await applicationDbContext.ProjectUsers.AddAsync(pu);
            }
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = model.CurProjectId, tab = "bordered-users" });
        }
    }
}
