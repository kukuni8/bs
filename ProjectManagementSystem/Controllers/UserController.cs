using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ApplicationDbContext applicationDbContext;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var model = new List<UserIndexViewModel>();
            foreach (var user in users)
            {
                var timespan = DateTime.Now - user.JobDate;
                var year = (timespan.Days / 365).ToString() + "年";
                var newUser = new UserIndexViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Department = user?.Department,
                    Job = user.Job,
                    TrueName = user.TrueName,
                    RoleName = user.RoleName,
                    JobYear = year,
                    Age = user.BirthDate == default ? " " : (DateTime.Now.Year - user.BirthDate.Year).ToString() + "岁",
                };
                model.Add(newUser);
            }

            return View(model);
        }

        public IActionResult AddUser()
        {
            UserAddViewModel userAddViewModel = new UserAddViewModel();

            return View(userAddViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserAddViewModel userAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAddViewModel);
            }

            var user = new ApplicationUser
            {
                UserName = userAddViewModel.UserName,
                Email = userAddViewModel.Email,
                BirthDate = userAddViewModel.BirthDate,
                About = userAddViewModel.About,
                Address = userAddViewModel.Address,
                TrueName = userAddViewModel.TrueName,
                Department = userAddViewModel.Department,
                Job = userAddViewModel.Job,
                JobDate = userAddViewModel.JobDate,
            };

            var result = await userManager.CreateAsync(user, userAddViewModel.Password);

            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync(userAddViewModel.RoleName);
                user.RoleName = role.Name;
                await userManager.UpdateAsync(user);
                await userManager.AddToRoleAsync(user, role.Name);
                return RedirectToAction("Index");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(userAddViewModel);

        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var newUser = new UserEditViewModel
            {

                Email = user.Email,
                BirthDate = user.BirthDate,
                About = user.About,
                Address = user.Address,
                TrueName = user.TrueName,
                Department = user.Department,
                Job = user.Job,
                JobDate = user.JobDate,
                RoleName = user.RoleName,
                Id = id
            };
            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            user.Email = model.Email;
            user.BirthDate = model.BirthDate;
            user.About = model.About;
            user.Address = model.Address;
            user.TrueName = model.TrueName;
            user.Department = model.Department;
            user.Job = model.Job;
            user.JobDate = model.JobDate;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles);
                var newRole = await _roleManager.FindByNameAsync(model.RoleName);
                await userManager.AddToRoleAsync(user, newRole.Name);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "更新用户信息时发生错误");
            return View(user);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, role);
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "删除用户时发生错误");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "用户找不到");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteUserFromProject(int userId, int projectId)
        {
            var pu = new ProjectUser
            {
                ApplicationUserId = userId,
                ProjectId = projectId,
            };
            applicationDbContext.ProjectUsers.Remove(pu);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("ProjectDetail", "Project", new { id = projectId, tab = "bordered-users" });
        }
    }
}
