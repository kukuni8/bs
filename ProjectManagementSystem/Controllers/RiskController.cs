using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Xml.Linq;

namespace ProjectManagementSystem.Controllers
{
    public class RiskController : Controller
    {

        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public RiskController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var risks = await applicationDbContext.Risks.ToListAsync();
            return View(risks);
        }

        public async Task<IActionResult> AddRisk(int projectId)
        {
            var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == projectId);
            var model = new RiskAddViewModel
            {
                ProjectId = projectId,
                Project = project,
                PutForward = await userManager.FindByNameAsync(User.Identity.Name),
                PutForwardId = (await userManager.FindByNameAsync(User.Identity.Name)).Id,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddRisk(RiskAddViewModel model)
        {
            var risk = new Risk();
            risk.Name = model.Name;
            risk.CreateDate = DateTime.Now;
            risk.Status = model.Status;
            risk.RiskType = model.RiskType;
            risk.Level = model.Level;
            risk.Incidence = model.Incidence;
            risk.Project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.ProjectId);
            risk.PutForwardId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
            await applicationDbContext.Risks.AddAsync(risk);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = model.ProjectId, tab = "bordered-risks" });
        }

        public async Task<IActionResult> EditRisk(int riskId)
        {
            var risk = await applicationDbContext.Risks.Include(a => a.Project).Include(a => a.PutForward).Include(a => a.Functionary).FirstOrDefaultAsync(x => x.Id == riskId);
            var model = new RiskEditViewModel
            {
                Id = riskId,
                Name = risk.Name,
                Incidence = risk.Incidence,
                Solution = risk.Solution,
                CreateDate = risk.CreateDate,
                Status = risk.Status,
                RiskType = risk.RiskType,
                Level = risk.Level,
                PutForwardId = risk.PutForwardId,
                PutForward = await userManager.FindByIdAsync(risk.PutForwardId),
                Functionary = risk.Functionary,
                FunctionaryId = risk.FunctionaryId,
                ProjectId = risk.ProjectId,
                Project = risk.Project,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRisk(RiskEditViewModel model)
        {
            var risk = await applicationDbContext.Risks.FirstOrDefaultAsync(a => a.Id == model.Id);
            risk.Id = model.Id;
            risk.Name = model.Name;
            risk.Incidence = model.Incidence;
            risk.Solution = model.Solution;
            risk.CreateDate = model.CreateDate;
            risk.Status = model.Status;
            risk.RiskType = model.RiskType;
            risk.Level = model.Level;
            risk.PutForwardId = model.PutForwardId;
            risk.PutForward = await userManager.FindByIdAsync(model.PutForwardId);
            risk.FunctionaryId = model.FunctionaryId;
            risk.Functionary = await userManager.FindByIdAsync(model.FunctionaryId);
            risk.ProjectId = model.ProjectId;
            risk.Project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.ProjectId);
            applicationDbContext.Risks.Update(risk);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = risk.ProjectId, tab = "bordered-risks" });
        }

        public async Task<IActionResult> DeleteRisk(int riskId)
        {
            var risk = await applicationDbContext.Risks.FirstOrDefaultAsync(a => a.Id == riskId);
            var projectId = risk.ProjectId;
            applicationDbContext.Risks.Remove(risk);
            applicationDbContext.SaveChanges();
            return RedirectToAction("ProjectDetail", "Project", new { id = projectId, tab = "bordered-risks" });
        }

        public IActionResult ReturnDetail(int projectId)
        {
            return RedirectToAction("ProjectDetail", "Project", new { id = projectId, tab = "bordered-risks" });
        }


    }
}
