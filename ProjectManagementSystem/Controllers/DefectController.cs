using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
	public class DefectController : Controller
	{
		private readonly ApplicationDbContext applicationDbContext;
		private readonly UserManager<ApplicationUser> userManager;
		public DefectController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
		{
			this.applicationDbContext = applicationDbContext;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var defects = await applicationDbContext.Defects.ToListAsync();
			return View(defects);
		}

		public async Task<IActionResult> AddDefect(int projectId)
		{
			var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == projectId);
			var model = new DefectAddViewModel
			{
				PutForward = await userManager.FindByNameAsync(User.Identity.Name),
				PutForwardId = (await userManager.FindByNameAsync(User.Identity.Name)).Id,
				ProjectId = projectId,
				Project = project,
			};

			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> AddDefect(DefectAddViewModel model)
		{
			var defect = new Defect
			{
				Name = model.Name,
				Description = model.Description,
				CreateDate = DateTime.Now,
				Solution = model.Solution,
				Type = model.Type,
				Status = model.Status,
				PutForwardId = model.PutForwardId,
				PutForward = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == model.PutForwardId),
				ProjectId = model.ProjectId,
				Project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.ProjectId),
			};
			await applicationDbContext.Defects.AddAsync(defect);
			await applicationDbContext.SaveChangesAsync();
			return RedirectToAction("ProjectDetail", "Project", new { id = model.ProjectId, tab = "bordered-defects" });
		}

		public async Task<IActionResult> EditDefect(int defectId)
		{
			var defect = await applicationDbContext.Defects.FirstOrDefaultAsync(x => x.Id == defectId);
			var model = new DefectEditViewModel
			{
				Id = defectId,
				Name = defect.Name,
				Description = defect.Description,
				CreateDate = defect.CreateDate,
				Solution = defect.Solution,
				Type = defect.Type,
				Status = defect.Status,
				PutForward = defect.PutForward,
				PutForwardId = defect.PutForwardId,
				ProjectId = defect.ProjectId,
				Project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == defect.ProjectId),
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EditDefect(DefectEditViewModel model)
		{
			var defect = await applicationDbContext.Defects.FirstOrDefaultAsync(a => a.Id == model.Id);
			defect.Name = model.Name;
			defect.Description = model.Description;
			defect.CreateDate = model.CreateDate;
			defect.Solution = model.Solution;
			defect.Type = model.Type;
			defect.Status = model.Status;
			defect.PutForward = model.PutForward;
			defect.PutForwardId = model.PutForwardId;
			defect.ProjectId = model.ProjectId;
			defect.Project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.ProjectId);
			defect.FunctionaryId = model.FunctionaryId;
			defect.Functionary = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.Id == defect.FunctionaryId);
			applicationDbContext.SaveChanges();
			return RedirectToAction("ProjectDetail", "Project", new { id = defect.ProjectId, tab = "bordered-defects" });
		}

		public async Task<IActionResult> DeleteDefect(int defectId)
		{
			var defect = await applicationDbContext.Defects.FirstOrDefaultAsync(a => a.Id == defectId);
			applicationDbContext.Defects.Remove(defect);
			applicationDbContext.SaveChanges();
			return RedirectToAction("ProjectDetail", "Project", new { id = defect.ProjectId, tab = "bordered-defects" });
		}
		public IActionResult ReturnDetail(int projectId)
		{
			return RedirectToAction("ProjectDetail", "Project", new { id = projectId, tab = "bordered-defects" });
		}
	}
}
