using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public DepartmentController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await applicationDbContext.Departments
                .Include(d => d.Users)
                .ToListAsync();
            var model = new DepartmentIndexViewModel
            {
                AddDepartment = new Department(),
                Departments = departments,
            };
            return View(model);
        }

        public async Task<IActionResult> AddDepartment([Bind("AddDepartment")] DepartmentIndexViewModel vm)
        {
            var d = vm.AddDepartment;
            await applicationDbContext.Departments.AddAsync(d);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var d = await applicationDbContext.Departments
                .Include(d => d.Users)
                .FirstOrDefaultAsync(d => d.Id == id);
            foreach (var user in d.Users)
            {
                user.Department = null;
            }
            applicationDbContext.Departments.Remove(d);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
