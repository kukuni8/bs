using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{

	public class RoleController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleController(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			var model = new RoleIndexViewModal()
			{
				Roles = new List<RoleInfo>(),
			};
			foreach(var role in roles)
			{
				var roleInfo = new RoleInfo
				{
					Id=role.Id,
					Name=role.Name,
				};
				var str=
			}

			return View(roles);
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

			var role = new IdentityRole
			{
				Name = roleAddViewModel.RoleName
			};

			var result = await _roleManager.CreateAsync(role);

			foreach (var item in roleAddViewModel.AuthorityDic)
			{
				if (item.Value)
				{
					await _roleManager.AddClaimAsync(role, new Claim(item.Key, item.Key));
				}
			}
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
				Id = id,
				RoleName = role.Name,
			};

			var claims = await _roleManager.GetClaimsAsync(role);
			foreach (var claim in claims)
			{
				if (roleEditViewModel.AuthorityDic.ContainsKey(claim.Type))
				{
					roleEditViewModel.AuthorityDic[claim.Type] = true;
				}
				else
				{
					roleEditViewModel.AuthorityDic[claim.Type] = false;
				}
			}
			return View(roleEditViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> EditRole(RoleEditViewModel roleEditViewModel)
		{
			var role = await _roleManager.FindByIdAsync(roleEditViewModel.Id);
			if (role != null)
			{

				role.Name = roleEditViewModel.RoleName;

				var result = await _roleManager.UpdateAsync(role);
				ModelState.AddModelError(string.Empty, "更新角色时出错");

				var claims = await _roleManager.GetClaimsAsync(role);
				foreach (var claim in claims)
				{
					await _roleManager.RemoveClaimAsync(role, claim);
				}
				foreach (var item in roleEditViewModel.AuthorityDic)
				{
					if (item.Value)
					{
						await _roleManager.AddClaimAsync(role, new Claim(item.Key, item.Key));
					}
				}
				if (result.Succeeded)
				{
					return RedirectToAction("Index");
				}
				return View(roleEditViewModel);
			}

			return RedirectToAction("Index");
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
