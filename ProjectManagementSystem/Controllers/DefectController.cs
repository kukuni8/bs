using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class DefectController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public DefectController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var defects = await applicationDbContext.Defects.ToListAsync();
            return View(defects);
        }

        public IActionResult AddDefect()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDefect(DefectAddViewModel model)
        {
            await applicationDbContext.Defects.AddAsync(model.Defect);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditDefect(int id)
        {
            var defect = await applicationDbContext.Defects.FirstOrDefaultAsync(x => x.Id == id);
            var model = new DefectEditViewModel
            {
                Defect = defect,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDefect(DefectEditViewModel model)
        {
            var defect = await applicationDbContext.Defects.FirstOrDefaultAsync(a => a.Id == model.Defect.Id);
            defect.Name = model.Defect.Name;
            defect.Description = model.Defect.Description;
            defect.CreateDate = model.Defect.CreateDate;
            defect.Solution = model.Defect.Solution;
            defect.Type = model.Defect.Type;
            defect.Status = model.Defect.Status;
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDefect(int id)
        {
            var defect = await applicationDbContext.Defects.FirstOrDefaultAsync(a => a.Id == id);
            applicationDbContext.Defects.Remove(defect);
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddTable()
        {
            applicationDbContext.DefectTypes.Add(new Models.DefectType { TypeName = "设计" });
            applicationDbContext.DefectTypes.Add(new Models.DefectType { TypeName = "技术" });
            applicationDbContext.DefectTypes.Add(new Models.DefectType { TypeName = "质量" });
            applicationDbContext.DefectStatuses.Add(new Models.DefectStatus { StatusName = "待处理" });
            applicationDbContext.DefectStatuses.Add(new Models.DefectStatus { StatusName = "进行中" });
            applicationDbContext.DefectStatuses.Add(new Models.DefectStatus { StatusName = "已完成" });
            applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
