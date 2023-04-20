using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class MissionController : Controller
    {
        private readonly DataDbContext dataDbContext;

        public MissionController(DataDbContext dataDbContext)
        {
            this.dataDbContext = dataDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var missions = await dataDbContext.Missions.ToListAsync();
            return View(missions);
        }

        public IActionResult AddMission()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMission(MissionAddViewModel model)
        {
            var mission = model.Mission;
            await dataDbContext.Missions.AddAsync(mission);
            dataDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMission(int id)
        {
            var mission = await dataDbContext.Missions.FirstOrDefaultAsync(x => x.Id == id);
            if (mission != null)
            {
                dataDbContext.Missions.Remove(mission);
                dataDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditMission(int id)
        {
            var mission = await dataDbContext.Missions.FirstOrDefaultAsync(a => a.Id == id);
            var model = new MissionEditViewModel
            {
                Mission = mission,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditMission(MissionEditViewModel model)
        {
            var mission = await dataDbContext.Missions.FirstOrDefaultAsync(a => a.Id == model.Mission.Id);
            mission.Name = model.Mission.Name;
            mission.Description = model.Mission.Description;
            mission.Type = model.Mission.Type;
            mission.CreateDate = model.Mission.CreateDate;
            mission.UpdateDate = model.Mission.UpdateDate;
            mission.Deadline = model.Mission.Deadline;
            mission.Priority = model.Mission.Priority;
            mission.Status = model.Mission.Status;
            mission.Executor = model.Mission.Executor;
            dataDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddTable()
        {
            dataDbContext.MissionStatuses.Add(new MissionStatus
            {
                Status = "待处理",
            });
            dataDbContext.MissionStatuses.Add(new MissionStatus
            {
                Status = "进行中",
            });
            dataDbContext.MissionStatuses.Add(new MissionStatus
            {
                Status = "已完成",
            });
            dataDbContext.SaveChanges();
            return View();
        }
    }
}
