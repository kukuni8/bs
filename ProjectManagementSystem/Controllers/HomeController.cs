using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Diagnostics;

namespace ProjectManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext applicationDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            if (applicationDbContext.MissionPriority.Count() == 0)
            {
                applicationDbContext.MissionStatuses.Add(new MissionStatus
                {
                    Status = "待处理",
                });
                applicationDbContext.MissionStatuses.Add(new MissionStatus
                {
                    Status = "进行中",
                });
                applicationDbContext.MissionStatuses.Add(new MissionStatus
                {
                    Status = "已完成",
                });
                applicationDbContext.MissionPriority.Add(new MissionPriority { Priority = "较低" });
                applicationDbContext.MissionPriority.Add(new MissionPriority { Priority = "普通" });
                applicationDbContext.MissionPriority.Add(new MissionPriority { Priority = "紧急" });
                applicationDbContext.SaveChanges();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}