using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Diagnostics;

namespace ProjectManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataDbContext dataDbContext;

        public HomeController(ILogger<HomeController> logger, DataDbContext dataDbContext)
        {
            _logger = logger;
            this.dataDbContext = dataDbContext;
        }

        public IActionResult Index()
        {
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