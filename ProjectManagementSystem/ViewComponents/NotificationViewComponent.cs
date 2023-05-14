using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class NotificationViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> userManager;

    public NotificationViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        this.userManager = userManager;
    }

    //public async Task<IViewComponentResult> InvokeAsync()
    //{

    //    if (User.Identity.Name != null)
    //    {
    //        var user = await userManager.FindByNameAsync(User.Identity.Name);
    //        var notices = await _context.Notices.Include(n => n.ApplicationUser).ToListAsync();
    //        var notifications = notices
    //            .Where(n => n.ApplicationUser.Id == user.Id)
    //            .OrderByDescending(n => n.CreateTime)
    //            .ToList();
    //        return View(notifications);
    //        // 其他操作
    //    }
    //    return View(new List<Notice>());


    //}


}
