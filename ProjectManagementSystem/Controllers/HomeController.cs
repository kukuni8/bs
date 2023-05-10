using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Diagnostics;

namespace ProjectManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // 检查用户是否已登录
            if (!User.Identity.IsAuthenticated)
            {
                return View(); // 或者返回其他视图，比如登录页面
            }

            var model = new HomeIndexViewModel();

            // 获取当前登录的用户
            var user = await userManager.GetUserAsync(User);

            // 查询当前用户的未读通知
            var notices = await applicationDbContext.Notices.Where(n => n.ApplicationUser.Id == user.Id && !n.IsRead).ToListAsync();

            // 将未读通知添加到 ViewModel
            model.Notifications = notices;

            return View(model);
        }

        public async Task<IActionResult> NotificationDropdown()
        {
            // 检查用户是否已登录
            if (!User.Identity.IsAuthenticated)
            {
                return PartialView("_NotificationDropdown", new List<Notice>());
            }

            // 获取当前登录的用户
            var user = await userManager.GetUserAsync(User);

            // 查询当前用户的未读通知
            var notices = await applicationDbContext.Notices
                .Where(n => n.ApplicationUser.Id == user.Id && !n.IsRead)
                .OrderByDescending(n => n.CreateTime)
                .ToListAsync();

            // 返回分布页视图并传递未读通知
            return PartialView("_NotificationDropdown", notices);
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