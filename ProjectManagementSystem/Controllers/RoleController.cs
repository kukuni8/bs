using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{

    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            this.userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var model = new RoleIndexViewModal()
            {
                Roles = new List<RoleInfo>(),
            };
            foreach (var role in roles)
            {
                var roleInfo = new RoleInfo
                {
                    Id = role.Id,
                    Name = role.Name,
                };
                var claims = await _roleManager.GetClaimsAsync(role);
                var str = "";
                for (int i = 0; i < claims.Count; i++)
                {
                    var claim = claims[i];
                    str += claim.Value.ToString();
                    if (i != claims.Count - 1)
                        str += ",";
                }
                roleInfo.Priority = str;
                model.Roles.Add(roleInfo);
            }

            return View(model);
        }

        public IActionResult AddRole()
        {
            RoleAddViewModel model = new RoleAddViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleAddViewModel roleAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(roleAddViewModel);
            }

            var role = new IdentityRole<int>
            {
                Name = roleAddViewModel.RoleName
            };

            var result = await _roleManager.CreateAsync(role);

            foreach (var item in roleAddViewModel.Infos)
            {
                foreach (var v in item.priorityInfos)
                {
                    if (v.IsOn)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(v.Name, v.Name));
                    }
                }
            }
            await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(roleAddViewModel);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction("Index");
            }

            var roleEditViewModel = new RoleEditViewModel
            {
                RoleName = role.Name,
                RoleId = role.Id
            };


            for (int i = 0; i < roleEditViewModel.Infos.Count; i++)
            {
                for (int j = 0; j < roleEditViewModel.Infos[i].priorityInfos.Count; j++)
                {
                    roleEditViewModel.Infos[i].priorityInfos[j].IsOn = (await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == roleEditViewModel.Infos[i].priorityInfos[j].Name);
                }
            }
            return View(roleEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleEditViewModel roleEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(roleEditViewModel);
            }

            var role = await _roleManager.FindByIdAsync(roleEditViewModel.RoleId.ToString());

            role.Name = roleEditViewModel.RoleName;

            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            foreach (var item in roleEditViewModel.Infos)
            {
                foreach (var v in item.priorityInfos)
                {
                    if (v.IsOn)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(v.Name, v.Name));
                    }
                }
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(roleEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "删除角色时出错");
            }
            ModelState.AddModelError(string.Empty, "没找到该角色");
            return View("Index", await _roleManager.Roles.ToListAsync());
        }
    }
}
