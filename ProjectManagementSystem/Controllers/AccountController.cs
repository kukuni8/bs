using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
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
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var modal = new AccountIndexViewModal { ApplicationUser = user };
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
}
