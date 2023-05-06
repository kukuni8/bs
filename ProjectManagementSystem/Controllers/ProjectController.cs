﻿using Microsoft.AspNetCore.Identity;
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

		public ProjectController(ApplicationDbContext applicationDbContext, SignInManager<ApplicationUser> signInManager)
		{
			this.applicationDbContext = applicationDbContext;
			this.signInManager = signInManager;
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
					CreatedDate = project.CreatedDate,
					Status = project.Status,
				};
				var curMissions = missions.Where(a => a.ProjectId == project.Id).ToList();
				var curMissionsCount = curMissions.Count() == 0 ? 1 : curMissions.Count();
				var unDealMissionCount = curMissions.Where(a => a.Status == MissionStatus.待处理 && a.Deadline >= DateTime.Now).Count();
				var compeleteMissionCount = curMissions.Where(a => a.Status == MissionStatus.进行中 && a.Deadline >= DateTime.Now).Count();
				var inprogressMissionCount = curMissions.Where(a => a.Status == MissionStatus.已完成 && a.Deadline >= DateTime.Now).Count();
				var timeoutMissionCount = curMissions.Where(a => a.Deadline < DateTime.Now).Count();
				model.UnDealPercent = unDealMissionCount * 100 / curMissionsCount;
				model.FinishedPercent = compeleteMissionCount * 100 / curMissionsCount;
				model.InProgressPercent = inprogressMissionCount * 100 / curMissionsCount;
				models.Add(model);
			}

			return View(models);
		}
		public async Task<IActionResult> ProjectDetail(int id)
		{
			var project = await applicationDbContext.Projects.Include(a => a.Risks).Include(a => a.Defects).Include(a => a.ApplicationUsers).FirstOrDefaultAsync(a => a.Id == id);
			var missions = await applicationDbContext.Missions.Include(a => a.Executors).Where(a => a.ProjectId == project.Id).ToListAsync();
			var model = new ProjectDetailViewModel()
			{
				ProjectEditViewModel = new ProjectEditViewModel()
				{
					Id = project.Id,
					Name = project.Name,
					Deadline = project.Deadline,
					Description = project.Description,
					CreatedDate = project.CreatedDate,
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

			model.DefectEditViewModels = project.Defects.Select(d => new DefectEditViewModel
			{
				Id = d.Id,
				Name = d.Name,
				Description = d.Description,
				CreateDate = d.CreateDate,
				Solution = d.Solution,
				Type = d.Type,
				Status = d.Status,
				PutForward = d.PutForward,
				PutForwardId = d.PutForwardId,
				Project = project,
				ProjectId = project.Id,
				Functionary = d.Functionary,
				FunctionaryId = d.FunctionaryId,
			});

			model.UserIndexModels = project.ApplicationUsers.Select(a => new ProjectUserIndexViewModel
			{
				Id = a.Id,
				Name = a.UserName,
				Job = a.Job,
				Department = a.Department,
				RoleName = a.RoleName,
			});

			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> EditProject([Bind("ProjectEditViewModel")] ProjectDetailViewModel model)
		{
			var project = await applicationDbContext.Projects.FirstOrDefaultAsync(a => a.Id == model.ProjectEditViewModel.Id);
			project.Name = model.ProjectEditViewModel.Name;
			project.Description = model.ProjectEditViewModel.Description;
			project.CreatedDate = model.ProjectEditViewModel.CreatedDate;
			project.UpdatedDate = DateTime.Now;
			project.Status = model.ProjectEditViewModel.Status;
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
			var putforward = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.PutForward);
			var functionary = await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(a => a.UserName == model.Functionary);
			var projectModel = new Project
			{
				Name = model.Name,
				Description = model.Description,
				CreatedDate = DateTime.Now,
				Deadline = DateTime.Now,
				FunctionaryId = functionary.Id,
				PutForwardId = putforward.Id,
				Status = (ProjectStatus)model.Status,
				ApplicationUsers = new List<ApplicationUser>(),
			};
			projectModel.ApplicationUsers.Add(putforward);
			if (!projectModel.ApplicationUsers.Contains(functionary))
			{
				projectModel.ApplicationUsers.Add(functionary);
			}
			applicationDbContext.Projects.Add(projectModel);
			applicationDbContext.SaveChanges();
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> DeleteProject(int id)
		{
			var project = await applicationDbContext.Projects.Include(a => a.ApplicationUsers).Include(a => a.Risks).Include(a => a.Defects).Include(a => a.Missions).FirstOrDefaultAsync(b => b.Id == id);

			applicationDbContext.Missions.RemoveRange(project.Missions);
			applicationDbContext.Risks.RemoveRange(project.Risks);
			applicationDbContext.Defects.RemoveRange(project.Defects);
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
				ProjectId = project.Id,
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
			mission.ProjectId = project.Id;
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
			var projectId = mission.ProjectId;
			applicationDbContext.Missions.Remove(mission);
			applicationDbContext.SaveChanges();
			return RedirectToAction("ProjectDetail", new { id = projectId });
		}
	}
}
