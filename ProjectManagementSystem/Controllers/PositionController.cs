using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class PositionController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public PositionController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var positions = await applicationDbContext.Positions
                .Include(p => p.Users)
                .ToListAsync();
            var model = new PositionIndexViewModel
            {
                AddPosition = new Position(),
                Positions = positions
            };
            return View(model);
        }

        public async Task<IActionResult> AddPosition([Bind("AddPosition")] PositionIndexViewModel vm)
        {
            var p = vm.AddPosition;
            await applicationDbContext.Positions.AddAsync(p);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePosition(int id)
        {
            var p = await applicationDbContext.Positions
                .Include(p => p.Users)
                .FirstOrDefaultAsync(d => d.Id == id);
            foreach (var user in p.Users)
            {
                user.Job = null;
            }
            applicationDbContext.Positions.Remove(p);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
