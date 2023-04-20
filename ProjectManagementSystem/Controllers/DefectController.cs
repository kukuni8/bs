using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    public class DefectController : Controller
    {
        private readonly DataDbContext dataDbContext;

        public DefectController(DataDbContext dataDbContext)
        {
            this.dataDbContext = dataDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var defects = await dataDbContext.Defects.ToListAsync();
            return View(defects);
        }

        public IActionResult AddDefect()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDefect(DefectAddViewModel model)
        {
            await dataDbContext.Defects.AddAsync(model.Defect);
            await dataDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditDefect(int id)
        {
            var defect = await dataDbContext.Defects.FirstOrDefaultAsync(x => x.Id == id);
            var model = new DefectEditViewModel
            {
                Defect = defect,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDefect(DefectEditViewModel model)
        {
            var defect = await dataDbContext.Defects.FirstOrDefaultAsync(a => a.Id == model.Defect.Id);
            defect.Name = model.Defect.Name;
            defect.Description = model.Defect.Description;
            defect.CreateDate = model.Defect.CreateDate;
            defect.Solution = model.Defect.Solution;
            defect.DefectType = model.Defect.DefectType;
            defect.DefectStatus = model.Defect.DefectStatus;
            dataDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDefect(int id)
        {
            var defect = await dataDbContext.Defects.FirstOrDefaultAsync(a => a.Id == id);
            dataDbContext.Defects.Remove(defect);
            dataDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddTable()
        {
            dataDbContext.DefectTypes.Add(new Models.DefectType { TypeName = "设计" });
            dataDbContext.DefectTypes.Add(new Models.DefectType { TypeName = "技术" });
            dataDbContext.DefectTypes.Add(new Models.DefectType { TypeName = "质量" });
            dataDbContext.DefectStatuses.Add(new Models.DefectStatus { StatusName = "待处理" });
            dataDbContext.DefectStatuses.Add(new Models.DefectStatus { StatusName = "进行中" });
            dataDbContext.DefectStatuses.Add(new Models.DefectStatus { StatusName = "已完成" });
            dataDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
