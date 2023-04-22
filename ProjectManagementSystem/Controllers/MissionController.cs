using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public async Task<IActionResult> Index()
        {
            var missions = await applicationDbContext.Missions.ToListAsync();
            MissionIndexViewModel model = new MissionIndexViewModel
            {
                UntreatedMissions = await applicationDbContext.Missions.Include(a => a.Status).Include(a => a.Executor).Include(a => a.Priority).Where(a => a.Status.Status == "待处理").ToListAsync(),
                ProcessOnMissions = await applicationDbContext.Missions.Include(a => a.Status).Include(a => a.Executor).Include(a => a.Priority).Where(a => a.Status.Status == "进行中").ToListAsync(),
                FinishedMissions = await applicationDbContext.Missions.Include(a => a.Status).Include(a => a.Executor).Include(a => a.Priority).Where(a => a.Status.Status == "已完成").ToListAsync(),
                EditMission = new Mission { }
            };
            return View(model);
        }

        public async Task<IActionResult> OnSelectOne(int id)
        {
            var currentMission = await applicationDbContext.Missions
                .Include(a => a.Status)
                .Include(a => a.Executor)
                .Include(a => a.Priority)
                .FirstOrDefaultAsync(a => a.Id == id);
            var res = new
            {
                id = currentMission.Id,
                name = currentMission.Name,
                description = currentMission.Description,
                createDate = currentMission.CreateDate,
                updateDate = currentMission.UpdateDate,
                deadline = currentMission.Deadline,
                priority = currentMission.Priority,
                status = currentMission.Status,
                executor = currentMission.Executor.Select(a => a.UserName).ToArray(),
                dialogues = currentMission.Dialogues,
            };
            //var options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve,
            //};

            //var jsonString = JsonSerializer.Serialize(res, options);
            return Json(res);
        }


        [HttpPost]
        public async Task<IActionResult> AddMission(MissionIndexViewModel model)
        {
            var mission = new Mission();
            mission.CreateDate = DateTime.Now;
            mission.Name = model.Name;
            mission.Description = model.Description;
            mission.Priority = await applicationDbContext.MissionPriority.FirstOrDefaultAsync(a => a.Priority == model.Priority);
            mission.Status = await applicationDbContext.MissionStatuses.FirstOrDefaultAsync(a => a.Status == model.Status);
            mission.Deadline = model.Deadline;
            var names = model.Executor.Split(',');
            foreach (var name in names)
            {
                mission.Executor ??= new List<ApplicationUser>();
                mission.Executor.Add(await userManager.Users.FirstOrDefaultAsync(a => a.UserName == name));
            }
            await applicationDbContext.Missions.AddAsync(mission);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<IActionResult> EditMission(MissionIndexViewModel model)
        {
            var mission = await applicationDbContext.Missions
                .Include(a => a.Status)
                .Include(a => a.Executor)
                .Include(a => a.Priority).FirstOrDefaultAsync(a => a.Id == model.EditId);
            mission.Name = model.EditName;
            mission.Description = model.EditDescription;

            mission.UpdateDate = DateTime.Now;
            mission.Deadline = model.EditDeadline;
            mission.Priority = await applicationDbContext.MissionPriority.FirstOrDefaultAsync(a => a.Priority == model.EditPriority);
            mission.Status = await applicationDbContext.MissionStatuses.FirstOrDefaultAsync(a => a.Status == model.EditStatus);
            mission.Executor.Clear();
            mission.Executor.AddRange(await userManager.Users.AsNoTracking().Where(a => model.EditExecutor.Contains(a.UserName)).ToListAsync());

            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
