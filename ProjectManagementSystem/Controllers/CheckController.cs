using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Reflection;

namespace ProjectManagementSystem.Controllers
{
    public class CheckController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public CheckController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> CheckMission(int id)
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

        public async Task<IActionResult> PassMission(int id)
        {
            var mission = await applicationDbContext.Missions
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            mission.CheckStatus = CheckStatus.审核通过;
            applicationDbContext.Missions.Update(mission);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-checks", accordionId = "missionaccordion-item" });
        }

        public async Task<IActionResult> UnPassMission(int id)
        {
            var mission = await applicationDbContext.Missions
                .Include(m => m.Project)
                .Include(m => m.Dialogues)
                .FirstOrDefaultAsync(m => m.Id == id);
            mission.CheckStatus = CheckStatus.审核未通过;
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var dia = new MissionDialogue
            {
                MissionId = mission.Id,
                Speaker = user,
                CreateDate = DateTime.Now,
                Content = $"任务审核未通过",
            };
            applicationDbContext.MissionDialogues.Add(dia);
            mission.Status = MissionStatus.进行中;
            var dia1 = new MissionDialogue
            {
                MissionId = mission.Id,
                Speaker = user,
                CreateDate = DateTime.Now,
                Content = $"将任务状态改为了进行中",
            };
            applicationDbContext.MissionDialogues.Add(dia1);

            applicationDbContext.Missions.Update(mission);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-checks", accordionId = "missionaccordion-item" });
        }

        public async Task<IActionResult> ReturnCheckMissionView(int id)
        {
            var mission = await applicationDbContext.Missions
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            return RedirectToAction("ProjectDetail", "Project", new { id = mission.Project.Id, tab = "bordered-checks", accordionId = "missionaccordion-item" });
        }

        public async Task<IActionResult> CheckRisk(int id)
        {
            var risk = await applicationDbContext.Risks
                .Include(a => a.Project)
                .Include(a => a.PutForward)
                .Include(a => a.Functionary)
                .FirstOrDefaultAsync(x => x.Id == id);
            var model = new RiskEditViewModel
            {
                Id = id,
                Name = risk.Name,
                Incidence = risk.Incidence,
                Solution = risk.Solution,
                CreateDate = risk.CreateDate,
                Status = risk.Status,
                RiskType = risk.RiskType,
                Level = risk.Level,
                PutForwardId = risk.PutForwardId,
                PutForward = await userManager.FindByIdAsync(risk.PutForwardId.ToString()),
                Functionary = risk.Functionary,
                FunctionaryId = risk.FunctionaryId,
                Project = risk.Project,
                ProjectId = risk.Project.Id,
            };
            return View(model);
        }

        public async Task<IActionResult> PassRisk(int id)
        {
            var risk = await applicationDbContext.Risks
                .Include(a => a.Project)
                .FirstOrDefaultAsync(r => r.Id == id);
            risk.Status = RiskStatus.待处理;
            risk.CheckStatus = CheckStatus.审核通过;
            applicationDbContext.Risks.Update(risk);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = risk.Project.Id, tab = "bordered-checks", accordionId = "riskaccordion-item" });
        }

        public async Task<IActionResult> UnPassRisk(int id)
        {
            var risk = await applicationDbContext.Risks
                .Include(a => a.Project)
                .FirstOrDefaultAsync(r => r.Id == id);
            risk.Status = RiskStatus.已丢弃;
            risk.CheckStatus = CheckStatus.审核未通过;
            applicationDbContext.Risks.Update(risk);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = risk.Project.Id, tab = "bordered-checks", accordionId = "riskaccordion-item" });
        }

        public async Task<IActionResult> ReturnCheckRiskView(int id)
        {
            var risk = await applicationDbContext.Risks
               .Include(m => m.Project)
               .FirstOrDefaultAsync(m => m.Id == id);
            return RedirectToAction("ProjectDetail", "Project", new { id = risk.Project.Id, tab = "bordered-checks", accordionId = "riskaccordion-item" });
        }


        public async Task<IActionResult> CheckDefect(int id)
        {
            var defect = await applicationDbContext.Defects
                .Include(a => a.Project)
                .Include(a => a.PutForward)
                .Include(a => a.Functionary)
                .FirstOrDefaultAsync(x => x.Id == id);
            var model = new DefectEditViewModel
            {
                Id = id,
                Name = defect.Name,
                Description = defect.Description,
                Solution = defect.Solution,
                CreateDate = defect.CreateDate,
                Status = defect.Status,
                Type = defect.Type,
                PutForwardId = defect.PutForwardId,
                PutForward = await userManager.FindByIdAsync(defect.PutForwardId.ToString()),
                Functionary = defect.Functionary,
                FunctionaryId = defect.FunctionaryId,
                Project = defect.Project,
                ProjectId = defect.Project.Id,
            };
            return View(model);
        }

        public async Task<IActionResult> PassDefect(int id)
        {
            var defect = await applicationDbContext.Defects
                .Include(a => a.Project)
                .FirstOrDefaultAsync(r => r.Id == id);
            defect.Status = DefectStatus.待处理;
            defect.CheckStatus = CheckStatus.审核通过;
            applicationDbContext.Defects.Update(defect);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = defect.Project.Id, tab = "bordered-checks", accordionId = "defectaccordion-item" });
        }

        public async Task<IActionResult> UnPassDefect(int id)
        {
            var defect = await applicationDbContext.Defects
                .Include(a => a.Project)
                .FirstOrDefaultAsync(r => r.Id == id);
            defect.Status = DefectStatus.已丢弃;
            defect.CheckStatus = CheckStatus.审核未通过;
            applicationDbContext.Defects.Update(defect);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = defect.Project.Id, tab = "bordered-checks", accordionId = "defectaccordion-item" });
        }

        public async Task<IActionResult> ReturnCheckDefectView(int id)
        {
            var defect = await applicationDbContext.Defects
               .Include(m => m.Project)
               .FirstOrDefaultAsync(m => m.Id == id);
            return RedirectToAction("ProjectDetail", "Project", new { id = defect.Project.Id, tab = "bordered-checks", accordionId = "defectaccordion-item" });
        }

    }
}
