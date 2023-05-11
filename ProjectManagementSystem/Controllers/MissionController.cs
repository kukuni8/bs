using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace ProjectManagementSystem.Controllers
{
    public class MissionController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public MissionController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int Status)
        {
            var missions = await applicationDbContext.Missions.Include(m => m.Project).Where(m => m.Status == (MissionStatus)Status).ToListAsync();
            var model = new MissionIndexViewModel
            {
                Missions = missions,
            };
            return View(model);
        }


        public async Task<IActionResult> MyIndex()
        {
            var missions = await applicationDbContext.Missions
                .Include(m => m.Project)
                .OrderByDescending(m => m.Status)
                .ToListAsync();
            var model = new MissionIndexViewModel
            {
                Missions = missions,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMission(int id)
        {
            var mission = await applicationDbContext.Missions.FirstOrDefaultAsync(x => x.Id == id);
            if (mission != null)
            {
                applicationDbContext.Missions.Remove(mission);
                applicationDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditMission(int id)
        {
            var mission = await applicationDbContext.Missions
                .Include(m => m.Project)
                .Include(m => m.PutForward)
                .Include(m => m.MissionExecutors).ThenInclude(me => me.ApplicationUser)
                .Include(m => m.Dialogues)
                .FirstOrDefaultAsync(m => m.Id == id);
            var model = new MissionEditViewModel
            {
                Id = mission.Id,
                Name = mission.Name,
                Description = mission.Description,
                CreateDate = mission.CreateDate,
                Deadline = mission.Deadline,
                Priority = mission.Priority,
                Status = mission.Status,
                StartDate = mission.StartDate,
                PutForward = mission.PutForward,
                Project = mission.Project,
                Executors = mission.MissionExecutors.Select(me => me.ApplicationUser.UserName).ToList(),
                Dialogues = mission.Dialogues,
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> EditMission(MissionEditViewModel model)
        {
            var mission = await applicationDbContext.Missions.Include(m => m.Dialogues).FirstOrDefaultAsync(m => m.Id == model.Id);
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
            return RedirectToAction("EditMission", new { id = mission.Id });
        }




    }
}
