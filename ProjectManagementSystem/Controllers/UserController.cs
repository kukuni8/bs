using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IImageService imageHelper;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, ApplicationDbContext applicationDbContext, IImageService imageHelper)
        {
            this.applicationDbContext = applicationDbContext;
            this.imageHelper = imageHelper;
            this.userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users
                .Include(u => u.Department)
                .Include(u => u.Job)
                .Include(u => u.PutForwardProjects)
                .Include(u => u.PutForwardRisks)
                .Include(u => u.PutForwardDefects)
                .Include(u => u.PutForwardMissions)
                .Include(u => u.PutforwardChats)
                .Include(u => u.FunctionaryDefects)
                .Include(u => u.FunctionaryProjects)
                .Include(u => u.FunctionaryRisks)
                .ToListAsync();
            var model = new List<UserIndexViewModel>();

            foreach (var user in users)
            {
                var canDelete = user.PutForwardProjects.Count > 0
                    || user.PutForwardRisks.Count > 0
                    || user.PutForwardDefects.Count > 0
                    || user.PutForwardMissions.Count > 0
                    || user.PutforwardChats.Count > 0
                    || user.FunctionaryProjects.Count > 0
                    || user.FunctionaryDefects.Count > 0
                    || user.FunctionaryRisks.Count > 0;
                var timespan = DateTime.Now - user.JobDate;
                var year = (timespan.Days / 365).ToString() + "年";
                var newUser = new UserIndexViewModel();

                newUser.Id = user.Id;
                newUser.UserName = user.UserName;
                newUser.Department = user.Department?.ToString();
                newUser.Job = user.Job?.ToString();
                newUser.TrueName = user.TrueName;
                newUser.RoleName = user.RoleName;
                newUser.JobYear = year;
                newUser.Age = user.BirthDate == default ? " " : (DateTime.Now.Year - user.BirthDate.Year).ToString() + "岁";
                newUser.ImagePath = user.ImagePath;
                newUser.CanDelete = !canDelete;
                model.Add(newUser);
            }

            return View(model);
        }

        [Authorize(Policy = "用户添加")]
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
                RoleName = userAddViewModel.RoleName,
                UserName = userAddViewModel.UserName,
                Email = userAddViewModel.Email,
                BirthDate = userAddViewModel.BirthDate,
                About = userAddViewModel.About,
                Address = userAddViewModel.Address,
                TrueName = userAddViewModel.TrueName,
                Department = await applicationDbContext.Departments.FirstOrDefaultAsync(d => d.Name == userAddViewModel.Department),
                Job = await applicationDbContext.Positions.FirstOrDefaultAsync(p => p.Name == userAddViewModel.Job),
                JobDate = userAddViewModel.JobDate,
                ImagePath = imageHelper.SaveCoverImage(userAddViewModel.Image),
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
                // 检查错误描述中是否包含有关重复用户名的信息
                if (error.Code == "DuplicateUserName")
                {
                    // 这个错误是由于用户名重复
                    ModelState.AddModelError(string.Empty, "用户名已存在。");
                }
                else
                {
                    // 其他错误
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(userAddViewModel);

        }

        [Authorize(Policy = "用户编辑")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var newUser = new UserEditViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                About = user.About,
                Address = user.Address,
                TrueName = user.TrueName,
                Department = user.Department?.ToString(),
                Job = user.Job?.ToString(),
                JobDate = user.JobDate,
                RoleName = user.RoleName,
                Id = id,
                ImagePath = user.ImagePath,
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
            user.RoleName = model.RoleName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.BirthDate = model.BirthDate;
            user.About = model.About;
            user.Address = model.Address;
            user.TrueName = model.TrueName;
            user.Department = await applicationDbContext.Departments.FirstOrDefaultAsync(d => d.Name == model.Department);
            user.Job = await applicationDbContext.Positions.FirstOrDefaultAsync(p => p.Name == model.Job);
            user.JobDate = model.JobDate;
            if (model.Image != null)
                user.ImagePath = imageHelper.SaveCoverImage(model.Image);


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

        [Authorize(Policy = "用户删除")]
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
