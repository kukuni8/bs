using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class FundController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext applicationDbContext;

        public FundController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }

        [Authorize(Policy = "新增收入")]
        [HttpPost]
        public async Task<IActionResult> AddFund([Bind("AddFundViewModel")] ResourceViewModel vm)
        {
            var model = vm.AddFundViewModel;
            var fundChange = new FundChange
            {
                Number = model.Number,
                DateTime = DateTime.Now,
                ChangeType = FundChangeType.收入,
                Description = model.Description,
                FundId = model.FundId,
                User = await userManager.FindByNameAsync(User.Identity.Name),
            };
            await applicationDbContext.FundChanges.AddAsync(fundChange);
            await applicationDbContext.SaveChangesAsync();
            var fund = await applicationDbContext.Funds
                .Include(f => f.Project)
                .FirstOrDefaultAsync(f => f.Id == model.FundId);
            applicationDbContext.Funds.Update(fund);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = fund.Project.Fund.Id, tab = "bordered-resources" });
        }

        [Authorize(Policy = "新增支出")]
        [HttpPost]
        public async Task<IActionResult> UseFund([Bind("UseFundViewModel")] ResourceViewModel vm)
        {
            var model = vm.UseFundViewModel;
            var fundChange = new FundChange
            {
                Number = model.Number,
                DateTime = DateTime.Now,
                ChangeType = FundChangeType.支出,
                Description = model.Description,
                FundId = model.FundId,
                User = await userManager.FindByNameAsync(User.Identity.Name),
            };
            await applicationDbContext.FundChanges.AddAsync(fundChange);
            await applicationDbContext.SaveChangesAsync();
            var fund = await applicationDbContext.Funds
                .Include(f => f.Project)
                .FirstOrDefaultAsync(f => f.Id == model.FundId);
            applicationDbContext.Funds.Update(fund);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = fund.Project.Fund.Id, tab = "bordered-resources" });
        }
    }
}
