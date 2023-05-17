using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly RoleManager<IdentityRole<int>> roleManager;
        public ProjectController(ApplicationDbContext applicationDbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager
          )
        {
            this.applicationDbContext = applicationDbContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await applicationDbContext.Projects
                .Include(a => a.Missions)
                .Include(p => p.PutForward)
                .Include(p => p.Functionary)
                .Include(p => p.ProjectUsers)
                .ToListAsync();
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
                    PutForward = project.PutForward,
                    Functionary = project.Functionary,
                    UserCount = project.ProjectUsers.Count(),
                };
                models.Add(model);
            }

            return View(models);
        }
        public async Task<IActionResult> ProjectDetail(int id)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.ApplicationUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            var model = new ProjectDetailViewModel()
            {
                CurProject = project,
                CurProjectId = project.Id,
            };
            model.UsersInTheProject = project.ProjectUsers.Select(a => a.ApplicationUser);
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> EditProject([Bind("ProjectEditViewModel")] ProjectViewModel model)
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
                Fund = new Fund(),
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

        public async Task<IActionResult> AddMission(MissionIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            var excutors = model.AddMission.Executors;
            var curMission = model.AddMission;
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == curMission.ProjectId);
            var user = (await userManager.FindByNameAsync(User.Identity.Name));
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
                PutForwardId = user.Id,
                MissionExecutors = new List<MissionExecutor>(),
            };
            await applicationDbContext.Missions.AddAsync(mission);
            applicationDbContext.SaveChanges(true);
            if (!string.IsNullOrEmpty(mission.Name))
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"创建了任务{mission.Name}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            if (!string.IsNullOrEmpty(mission.Priority.ToString()))
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"把任务优先级设置为{mission.Priority}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            if (!string.IsNullOrEmpty(mission.Status.ToString()))
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"把任务状态设置为{mission.Status}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            if (true)
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"把任务截止设置为{mission.Deadline}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var userName in excutors)
            {
                var curUser = await applicationDbContext.Users.FirstOrDefaultAsync(a => a.UserName == userName);
                var me = new MissionExecutor
                {
                    ApplicationUser = curUser,
                    Mission = mission,
                };
                mission.MissionExecutors.Add(me);
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"把{curUser.UserName}加入了任务",
                };
                applicationDbContext.MissionDialogues.Add(dia);
                applicationDbContext.Missions.Update(mission);

            }
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-missions" });
        }

        public async Task<IActionResult> EditMission([Bind("EditMission")] MissionIndexViewModel proModel)
        {
            var model = proModel.EditMission;
            var mission = await applicationDbContext.Missions.Include(m => m.Project).Include(m => m.Dialogues).FirstOrDefaultAsync(m => m.Id == model.Id);
            var creaeDate = DateTime.Now;

            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (mission.Name != model.Name)
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"将任务名称改为{model.Name}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }

            if (mission.Description != model.Description)
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"修改了任务描述:{model.Description}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            if (mission.Deadline.Date != model.Deadline.Date)
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"将任务截止时间改为{model.Deadline.ToString("yyyy-MM-dd")}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            if (mission.Priority != model.Priority)
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"将任务优先级设置为{model.Priority.ToString()}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            if (mission.Status != model.Status)
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = $"将任务状态设置为{model.Status.ToString()}",
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            if (model.Status == MissionStatus.已完成 && mission.CheckStatus == CheckStatus.审核未通过)
            {
                mission.CheckStatus = CheckStatus.再次审核;
            }
            var excuterNames = await applicationDbContext.MissionExecutors
                .Where(me => me.MissionId == mission.Id)
                .Select(me => me.ApplicationUser.UserName)
                .ToListAsync();
            foreach (var name in excuterNames)
            {
                if (!model.Executors.Contains(name))
                {
                    var dia = new MissionDialogue
                    {
                        MissionId = mission.Id,
                        Speaker = user,
                        CreateDate = DateTime.Now,
                        Content = $"将用户{name}移出了该任务",
                    };
                    applicationDbContext.MissionDialogues.Add(dia);
                }
            }
            foreach (var userName in model.Executors)
            {
                if (!excuterNames.Contains(userName))
                {
                    var dia = new MissionDialogue
                    {
                        MissionId = mission.Id,
                        Speaker = user,
                        CreateDate = DateTime.Now,
                        Content = $"将用户{userName}添加到了该任务",
                    };
                    applicationDbContext.MissionDialogues.Add(dia);
                }
            }
            if (!string.IsNullOrEmpty(model.Content))
            {
                var dia = new MissionDialogue
                {
                    MissionId = mission.Id,
                    Speaker = user,
                    CreateDate = DateTime.Now,
                    Content = model.Content,
                };
                applicationDbContext.MissionDialogues.Add(dia);
            }
            mission.Name = model.Name;
            mission.Description = model.Description;
            mission.Deadline = model.Deadline;
            mission.Priority = model.Priority;
            mission.Status = model.Status;
            var missionExcuters = await applicationDbContext.MissionExecutors
                .Where(me => me.MissionId == mission.Id)
                .ToListAsync();
            foreach (var ex in missionExcuters)
            {
                applicationDbContext.MissionExecutors.Remove(ex);
            }
            foreach (var ex in model.Executors)
            {
                var au = await userManager.FindByNameAsync(ex);
                var me = new MissionExecutor
                {
                    MissionId = mission.Id,
                    ApplicationUserId = au.Id,
                };
                await applicationDbContext.MissionExecutors.AddAsync(me);
            }
            applicationDbContext.Missions.Update(mission);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-missions" });
        }

        public async Task<IActionResult> DeleteMission(int id)
        {
            var missionId = id;
            var mission = await applicationDbContext.Missions
                .Include(m => m.MissionExecutors)
                .Include(m => m.Dialogues)
                .Include(m => m.Project)
                .FirstOrDefaultAsync(c => c.Id == missionId);
            var projectId = mission.Project.Id;
            var dias = mission.Dialogues;
            applicationDbContext.MissionDialogues.RemoveRange(dias);
            var mes = mission.MissionExecutors;
            applicationDbContext.MissionExecutors.RemoveRange(mes);
            var res = applicationDbContext.Missions.Remove(mission);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-missions" });
        }

        public async Task<IActionResult> AddUserToProject(UserViewModel model)
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
        [HttpGet]
        public async Task<JsonResult> GetData(int projectId)
        {
            var missions = await applicationDbContext.Missions
                .Include(m => m.Project)
                .Where(p => p.Project.Id == projectId)
                .ToListAsync();
            var startDay = missions.Select(m => m.CreateDate).Min();
            var endDay = missions.Select(m => m.FinishedTime).Max();
            var undealData = new List<(DateTime, int)>();
            var inprogressData = new List<(DateTime, int)>();
            var finishedData = new List<(DateTime, int)>();
            for (DateTime i = startDay; i <= endDay; i = i.AddDays(1))
            {
                undealData.Add((i, CalUndeal(missions, i)));
                inprogressData.Add((i, CalInprogress(missions, i)));
                finishedData.Add((i, CalFinished(missions, i)));
            }

            var data = new[]
            {
              new
                 {
                   name = "待处理",
                   color = "#4154f1", // 颜色字段
                   data = undealData.Select(d => new { x = d.Item1.ToString("o"), y = d.Item2 }).ToArray()
                 },
              new
                 {
                   name = "进行中",
                   color = "#2eca6a", // 颜色字段
                   data = inprogressData.Select(d => new { x = d.Item1.ToString("o"), y = d.Item2 }).ToArray()
                 },
              new
                 {
                   name = "已完成",
                   color = "#ff771d",
                   data = finishedData.Select(d => new { x = d.Item1.ToString("o"), y = d.Item2 }).ToArray()
                  }
             };


            return Json(data);
        }

        public async Task<JsonResult> GetStackColumnData(string user, int projectId)
        {
            var missions = await applicationDbContext.Missions
                .Include(m => m.MissionExecutors)
                .ThenInclude(me => me.ApplicationUser)
                .Include(m => m.Project)
                .Where(m => m.Project.Id == projectId)
                .ToListAsync();
            if (user != "All")
            {
                missions = missions.Where(m => m.MissionExecutors.Where(me => me.ApplicationUser.UserName == user).Count() > 0).ToList();
            }
            var undealData = new List<(DateTime, int)>();
            var inProgressData = new List<(DateTime, int)>();
            var FinishedData = new List<(DateTime, int)>();
            for (int i = 0; i < 40; i++)
            {
                var dateTime = DateTime.Now.AddDays(-i);
                undealData.Add((dateTime, CalCulateUndeal(missions, dateTime)));
                inProgressData.Add((dateTime, CalCulateInProgress(missions, dateTime)));
                FinishedData.Add((dateTime, CalCulateFinished(missions, dateTime)));
            }

            var data = new[]
  {
        new
        {
            name = "待处理",
            color = "#4154f1", // 颜色字段
            data = undealData.Select(d => new { x = d.Item1.ToString("o"), y = d.Item2 }).ToArray()
        },
        new
        {
            name = "进行中",
            color = "#2eca6a", // 颜色字段
            data = inProgressData.Select(d => new { x = d.Item1.ToString("o") , y = d.Item2 }).ToArray()
        },
        new
        {
            name = "已完成",
            color = "#ff771d",
            data = FinishedData.Select(d => new { x = d.Item1.ToString("o") , y = d.Item2 }).ToArray()
        }
    };

            return Json(data);
        }

        public async Task<IActionResult> GetFundData(int projecctId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.Fund)
                .ThenInclude(f => f.Changes)
                .FirstOrDefaultAsync(p => p.Id == projecctId);
            var changes = project.Fund.Changes.OrderBy(c => c.DateTime).ToList();
            var startDay = changes.Select(c => c.DateTime).Min();
            var endDay = changes.Select(c => c.DateTime).Max();
            decimal fund = 0M;
            var fundData = new List<(DateTime, decimal)>();
            var addData = new List<(DateTime, decimal)>();
            var reduceData = new List<(DateTime, decimal)>();
            foreach (var change in changes)
            {
                if (change.ChangeType == FundChangeType.支出)
                {
                    fund -= change.Number;
                    reduceData.Add((change.DateTime.Date, change.Number));
                    addData.Add((change.DateTime.Date, 0M));
                }
                else
                {
                    fund += change.Number;
                    addData.Add((change.DateTime.Date, change.Number));
                    reduceData.Add((change.DateTime.Date, 0M));
                }
                fundData.Add((change.DateTime.Date, fund));
            }

            var data = new[]
            {
                 new
            {
            name = "资金",
            color = "#00008B", // 颜色字段
            type="line",
            data = fundData.Select(d => new { x = d.Item1.ToString("o"), y = d.Item2 }).ToArray()
            },
                  new
            {
            name = "收入",
            color = "#3CB371", // 颜色字段
            type= "column",
            data = addData.Select(d => new { x = d.Item1.ToString("o"), y = d.Item2 }).ToArray()
            },
                   new
            {
            name = "支出",
            color = "#FF6347", // 颜色字段
            type= "column",
            data = reduceData.Select(d => new { x = d.Item1.ToString("o"), y = d.Item2}).ToArray()
            },
            };

            return Json(data);
        }

        public async Task<IActionResult> GetPieData(int projectId)
        {
            var me = await applicationDbContext.MissionExecutors
            .Include(me => me.ApplicationUser)
            .Include(me => me.Mission)
            .ThenInclude(m => m.Project)
            .Where(me => me.Mission.Project.Id == projectId && me.Mission.FinishedTime < DateTime.Now)
            .ToListAsync();

            var groupResult = me.GroupBy(me => me.ApplicationUser.UserName);
            var pieData = new List<(string, int)>();
            int count = 0;
            var total = me.Count;
            foreach (var group in groupResult)
            {
                pieData.Add((group.Key, group.Count()));
                count += group.Count();
                if ((float)count / total > 0.8f)
                    break;
            }
            pieData.Add(("Others", total - count));
            var data = new
            {
                names = pieData.Select(p => p.Item1).ToArray(),
                numbers = pieData.Select(p => p.Item2).ToArray(),
            };
            return Json(data);
        }

        public async Task<IActionResult> GetBarData(int projectId)
        {
            var me = await applicationDbContext.MissionExecutors
            .Include(me => me.ApplicationUser)
            .Include(me => me.Mission)
            .ThenInclude(m => m.Project)
            .Where(me => me.Mission.Project.Id == projectId && me.Mission.FinishedTime < DateTime.Now)
            .ToListAsync();

            var groupResult = me
                .GroupBy(me => me.ApplicationUser.UserName)
                .OrderByDescending(g => g.Count());
            var pieData = new List<(string, int)>();
            foreach (var group in groupResult)
            {
                pieData.Add((group.Key, group.Count()));
            }
            var data = new
            {
                names = pieData.Select(p => p.Item1).ToArray(),
                numbers = pieData.Select(p => p.Item2).ToArray(),
            };
            return Json(data);
        }

        private int CalUndeal(List<Mission> missions, DateTime dateTime)
        {
            return missions.Where(m => m.CreateDate.Date < dateTime.Date && m.StartDate > dateTime.Date).Count();
        }

        public int CalInprogress(List<Mission> missions, DateTime dateTime)
        {
            return missions.Where(m => m.StartDate.Date < dateTime.Date && m.FinishedTime.Date > dateTime.Date).Count();
        }

        public int CalFinished(List<Mission> missions, DateTime dateTime)
        {
            return missions.Where(m => m.FinishedTime.Date < dateTime.Date).Count();
        }

        private int CalCulateUndeal(List<Mission> missions, DateTime dateTime)
        {
            return missions.Where(m => m.CreateDate.Date == dateTime.Date).Count();
        }

        private int CalCulateInProgress(List<Mission> missions, DateTime dateTime)
        {
            return missions.Where(m => m.StartDate.Date == dateTime.Date).Count();
        }

        private int CalCulateFinished(List<Mission> missions, DateTime dateTime)
        {
            return missions.Where(m => m.FinishedTime.Date == dateTime.Date).Count();
        }




        public async Task<IActionResult> GetLookData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.ApplicationUser)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            var model = new LookIndexViewModel
            {
                ProjectId = projectId,
                UserNames = new List<string>(),
                TrueUserNames = new List<string>(),
            };
            model.UserNames.Add("All");
            model.UserNames.AddRange(project.ProjectUsers.Select(pu => pu.ApplicationUser.UserName).ToList());
            model.TrueUserNames.AddRange(project.ProjectUsers.Select(pu => pu.ApplicationUser.UserName).ToList());
            return PartialView("_LookPartialView", model);
        }
        public async Task<IActionResult> GetNoticeData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.Notices)
                .ThenInclude(n => n.Putforward)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            var model = new NoticeIndexViewModel
            {
                Notices = project.Notices.OrderByDescending(n => n.CreateTime),
                NoticeAddViewModel = new NoticeAddViewModel()
                {
                    NoticeType = NoticeType.Info,
                    Information = "",
                    ProjectId = project.Id,
                }
            };
            return PartialView("_NoticePartialView", model);
        }
        public IActionResult GetChatData(int projectId)
        {
            return PartialView("_ChatPartialView");
        }
        public async Task<IActionResult> GetMissionData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.Missions)
                .ThenInclude(m => m.PutForward)
                .Include(p => p.Missions)
                .ThenInclude(m => m.MissionExecutors)
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.ApplicationUser)
                .Include(p => p.Missions)
                .ThenInclude(m => m.Dialogues)
                .ThenInclude(d => d.Speaker)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            var missions = project.Missions;
            var model = new MissionIndexViewModel
            {
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
                                            Dialogues = a.Dialogues,
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
                                            Dialogues = a.Dialogues,
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
                                            Dialogues = a.Dialogues,
                                        }),
                },

                AddMission = new ProjectAddMissionViewModel
                {
                    Deadline = DateTime.Now,
                    Executors = new List<string>(),
                },
                EditMission = new ProjectEditMissionViewModel { },
                CurProjectId = project.Id,
                CurProjectName = project.Name,
            };

            model.UsersInTheProject = project.ProjectUsers.Select(a => a.ApplicationUser);

            return PartialView("_MissionPartialView", model);
        }
        public async Task<IActionResult> GetRiskData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.Risks)
                .ThenInclude(r => r.PutForward)
                .Include(p => p.Risks)
                .ThenInclude(r => r.Functionary)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            var model = new RiskIndexViewModel
            {
                CurProjectId = project.Id,
                CurProjectName = project.Name,
                UnCheckRiskViewModels = project.Risks
                .Where(r => r.Status == RiskStatus.审核中)
                .Select(r => new RiskEditViewModel
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
                }),

                UnDealRiskViewModels = project.Risks
               .Where(r => r.Status == RiskStatus.待处理)
               .Select(r => new RiskEditViewModel
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
               }),

                SettledRiskViewModels = project.Risks
               .Where(r => r.Status == RiskStatus.已解决)
               .Select(r => new RiskEditViewModel
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
               }),

                DiscardedRiskViewModels = project.Risks
               .Where(r => r.Status == RiskStatus.已丢弃)
               .Select(r => new RiskEditViewModel
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
               }),
            };
            return PartialView("_RiskPartialView", model);
        }
        public async Task<IActionResult> GetDefectData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.Defects)
                .ThenInclude(d => d.PutForward)
                .Include(p => p.Defects)
                .ThenInclude(d => d.Functionary)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            var model = new DefectIndexViewModel
            {
                CurProjectId = project.Id,
                CurProjectName = project.Name,
                DefectEditViewModels = project.Defects.Select(d => new DefectEditViewModel
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
                }),
            };
            return PartialView("_DefectPartialView", model);
        }
        public async Task<IActionResult> GetCheckData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(P => P.Missions)
                .Include(p => p.Defects)
                .Include(p => p.Risks)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            var model = new CheckIndexViewModel()
            {
                MissionChecks = project.Missions
                .Where(m => m.Status == MissionStatus.已完成)
                .OrderByDescending(m => m.CheckStatus)
                .Select(m => new CheckInfoViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Status = m.CheckStatus,
                }).ToList(),
                RiskChecks = project.Risks
                .Where(r => r.Status == RiskStatus.审核中 || r.CheckStatus == CheckStatus.审核通过 || r.CheckStatus == CheckStatus.审核未通过)
                .Select(m => new CheckInfoViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Status = m.CheckStatus,
                }).ToList(),
                DefectChecks = project.Defects
                .Where(d => d.Status == DefectStatus.审核中 || d.CheckStatus == CheckStatus.审核通过 || d.CheckStatus == CheckStatus.审核未通过)
                .Select(d => new CheckInfoViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Status = d.CheckStatus,
                }).ToList(),
            };
            return PartialView("_CheckPartialView", model);
        }
        public async Task<IActionResult> GetResourceData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.Resources)
                .ThenInclude(r => r.Changes)
                .ThenInclude(c => c.User)
                .Include(p => p.Fund)
                .ThenInclude(r => r.Changes)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            var model = new ResourceViewModel();
            model.ResourceIndexViewModels = project.Resources.Select(r => new ResourceIndexViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ProjectId = project.Id,
                Number = r.Number,
                ImagePath = r.ImagePath,
                ResourceChanges = r.Changes,
            });
            var paid = project.Fund.Changes
                .Where(c => c.ChangeType == FundChangeType.支出)
                .Sum(c => c.Number);
            var incom = project.Fund.Changes
                .Where(c => c.ChangeType == FundChangeType.收入)
                .Sum(c => c.Number);
            model.FundIndexViewModel = new FundIndexViewModel()
            {
                Id = project.Fund.Id,
                Amount = project.Fund.Amount,
                Changes = project.Fund.Changes,
                Paid = paid,
                Income = incom,
            };
            model.CurProjectId = project.Id;
            return PartialView("_ResourcePartialView", model);
        }
        public async Task<IActionResult> GetUserData(int projectId)
        {
            var projectUsers = await applicationDbContext.ProjectUsers
                .Include(pu => pu.Project)
                .Include(pu => pu.ApplicationUser)
                .ThenInclude(u => u.Department)
                .Include(pu => pu.ApplicationUser)
                .ThenInclude(u => u.Job)
                .Where(pu => pu.Project.Id == projectId)
                .ToListAsync();
            var model = new UserViewModel();
            model.ProjectUserIndexViewModels = projectUsers.Select(u => new ProjectUserIndexViewModel
            {
                Id = u.ApplicationUser.Id,
                Name = u.ApplicationUser.UserName,
                Department = u.ApplicationUser.Department?.ToString(),
                Job = u.ApplicationUser.Job?.ToString(),
                RoleName = u.ApplicationUser.RoleName,
            });
            var notin = await applicationDbContext.Users
                .Include(u => u.Department)
                .Include(u => u.Job)
                .Where(u => !projectUsers.Select(a => a.ApplicationUserId).Contains(u.Id)).ToListAsync();

            model.UsersNotInThisProject = notin.Select(u => new ProjectUserNotInProjectModel
            {
                Id = u.Id,
                Name = u.UserName,
                Department = u.Department?.ToString(),
                Job = u.Job?.ToString(),
                RoleName = u.RoleName,
                IsSelected = false,
            }).ToList();
            model.CurProjectId = projectId;
            return PartialView("_UserPartialView", model);
        }
        public async Task<IActionResult> GetBookData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.Books)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            var model = new BookViewModel();
            model.BookIndexViewModels = project.Books.Select(b => new BookIndexViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Content = b.Content,
                CoverImage = b.CoverImage,
                ProjectId = project.Id,
                Summary = b.Summary,
            });
            model.CurProjectId = project.Id;
            return PartialView("_BookPartialView", model);
        }
        public async Task<IActionResult> GetProjectData(int projectId)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.PutForward)
                .Include(p => p.Functionary)
                .FirstOrDefaultAsync(p => p.Id == projectId);
            var model = new ProjectViewModel
            {
                CurProjectId = project.Id,
                ProjectEditViewModel = new ProjectEditViewModel()
                {
                    Id = project.Id,
                    Name = project.Name,
                    Deadline = project.Deadline,
                    Description = project.Description,
                    CreatedDate = project.CreateDate,
                    Functionary = project.Functionary.UserName,
                    PutForward = project.PutForward.UserName,
                },
            };
            return PartialView("_EditProjectPartialView", model);
        }
    }
}
