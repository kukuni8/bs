using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;


namespace ProjectManagementSystem.Controllers
{
    public class PersonController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext applicationDbContext;

        public PersonController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var missions = await applicationDbContext.Missions.ToListAsync();
            var model = new PersonIndexViewModel
            {
                UndealMissionCount = missions.Where(m => m.Status == MissionStatus.待处理).Count(),
                InprogressMissionCount = missions.Where(m => m.Status == MissionStatus.进行中).Count(),
                FinishedMissionCount = missions.Where(m => m.Status == MissionStatus.已完成).Count(),
            };
            return View(model);
        }
    }
}
