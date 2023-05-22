using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using ProjectManagementSystem.Hubs;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagementSystem.Controllers
{

    public class NoticeController : Controller
    {
        private readonly IHubContext<NotificationHub> notifacationhubContext;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public NoticeController(IHubContext<NotificationHub> notifacationhubContext,
            ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.notifacationhubContext = notifacationhubContext;
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }
        [Authorize(Policy = "通知添加")]
        public async Task<IActionResult> AddNotice([Bind("NoticeAddViewModel")] NoticeIndexViewModel vm)
        {
            var model = vm.NoticeAddViewModel;
            var project = await applicationDbContext.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.ApplicationUser)
                .FirstOrDefaultAsync(p => p.Id == model.ProjectId);
            var notice = new Notice
            {
                Project = project,
                IsRead = false,
                CreateTime = DateTime.Now,
                Information = model.Information,
                Putforward = await userManager.GetUserAsync(User),
                NoticeType = model.NoticeType
            };
            await applicationDbContext.Notices.AddAsync(notice);
            await applicationDbContext.SaveChangesAsync();
            var noticeDto = new NoticeDTO
            {
                Id = notice.Id,
                IsRead = false,
                CreateTime = DateTime.Now,
                Information = model.Information,
                Putforward = User.Identity.Name,
                NoticeType = model.NoticeType.ToString() // 将 NoticeType 转换为字符串
            };
            foreach (var user in project.ProjectUsers)
            {
                var nr = new NoticeReceiver
                {
                    Notice = notice,
                    Receiver = user.ApplicationUser,
                };

                await notifacationhubContext.Clients.User(user.ApplicationUser.Id.ToString()).SendAsync("ReceiveNotice", noticeDto);
                await applicationDbContext.NoticeReceivers.AddAsync(nr);
            }

            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = model.ProjectId, tab = "bordered-notices" });
        }

        [HttpPost]
        public void MarkNoticeRead(int id)
        {
            var notice = applicationDbContext.Notices.FirstOrDefault(n => n.Id == id);
            if (notice != null)
            {
                notice.IsRead = true;
                applicationDbContext.SaveChanges();
            }
        }

    }
}
