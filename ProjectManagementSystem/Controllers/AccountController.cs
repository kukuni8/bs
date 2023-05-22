using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Security.Claims;

namespace ProjectManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IImageService imageService;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext applicationDbContext,
            SignInManager<ApplicationUser> signInManager,
            IImageService imageService,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.imageService = imageService;
            this.roleManager = roleManager;
            _context = applicationDbContext;
        }
        public async Task<IActionResult> Login()
        {
            var adminInit = new AdminInit(_context, _userManager, roleManager);
            await adminInit.Init();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "用户名/密码不正确");
            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser { UserName = model.UserName, };
            user.Email = model.Email;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "用户名重复，换一个试试!");
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var nUser = await _context.ApplicationUsers
                .Include(u => u.Department)
                .Include(u => u.Job)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            var modal = new AccountIndexViewModal { ApplicationUser = nUser };
            modal.SelectedTab = "profile-overview";
            return View(modal);
        }

        [HttpPost]
        public async Task<IActionResult> EditDetail(AccountIndexViewModal viewModal)
        {
            var model = viewModal.ApplicationUser;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            viewModal.SelectedTab = "profile-edit";
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Address = model.Address;
            user.About = model.About;
            user.Department = model.Department;
            user.Job = model.Job;
            user.TrueName = model.TrueName;
            user.PhoneNumber = model.PhoneNumber;
            if (viewModal.Image != null)
            {
                user.ImagePath = imageService.SaveCoverImage(viewModal.Image);
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Index", viewModal);
        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(AccountIndexViewModal viewModal)
        {
            var user = await _userManager.GetUserAsync(User);
            viewModal.ApplicationUser = user;
            viewModal.SelectedTab = "profile-change-password";
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, viewModal.OldPassword);
            if (!isPasswordCorrect)
            {
                ModelState.AddModelError("OldPassword", "当前密码不正确");
                return View("Index", viewModal);
            }
            // 更新密码
            var result = await _userManager.ChangePasswordAsync(user, viewModal.OldPassword, viewModal.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // 密码更改失败
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Index", viewModal);
            }
        }
    }


    public class AdminInit
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public AdminInit(ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task Init()
        {
            if (applicationDbContext.ApplicationUsers.Count() > 0)
                return;
            var user = new ApplicationUser();
            user.UserName = "admin";
            user.TrueName = "admin";
            user.Email = "2813550278@qq.com";
            //user.ImagePath=""
            var result = await userManager.CreateAsync(user, "admin");
            if (result.Succeeded)
            {
                await RoleInit();
                var role = await roleManager.FindByNameAsync("管理员");
                user.RoleName = role.Name;
                await userManager.UpdateAsync(user);
                await userManager.AddToRoleAsync(user, role.Name);
                await applicationDbContext.SaveChangesAsync();
            }
        }
        public async Task RoleInit()
        {
            var role = new IdentityRole<int>
            {
                Name = "管理员",
            };

            var result = await roleManager.CreateAsync(role);
            var roleAddViewModel = new RoleAddViewModel();

            foreach (var item in roleAddViewModel.Infos)
            {
                foreach (var v in item.priorityInfos)
                {
                    await roleManager.AddClaimAsync(role, new Claim(v.Name, v.Name));
                }
            }
        }
    }
}
