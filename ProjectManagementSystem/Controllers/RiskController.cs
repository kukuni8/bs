//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ProjectManagementSystem.Data;
//using ProjectManagementSystem.ViewModels;

//namespace ProjectManagementSystem.Controllers
//{
//    public class RiskController : Controller
//    {

//        private readonly ApplicationDbContext applicationDbContext;

//        public RiskController(ApplicationDbContext applicationDbContext)
//        {
//            this.applicationDbContext = applicationDbContext;
//        }
//        public async Task<IActionResult> Index()
//        {
//            var risks = await applicationDbContext.Risks.ToListAsync();
//            return View(risks);
//        }

//        public IActionResult AddRisk()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddRisk(RiskAddViewModel model)
//        {
//            await applicationDbContext.Risks.AddAsync(model.Risk);
//            await applicationDbContext.SaveChangesAsync();
//            return RedirectToAction("Index");
//        }

//        public async Task<IActionResult> EditRisk(int id)
//        {
//            var risk = await applicationDbContext.Risks.FirstOrDefaultAsync(x => x.Id == id);
//            var model = new RiskEditViewModel
//            {
//                Risk = risk,
//            };
//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditRisk(RiskEditViewModel model)
//        {
//            var risk = await applicationDbContext.Risks.FirstOrDefaultAsync(a => a.Id == model.Risk.Id);
//            risk.Name = model.Risk.Name;
//            risk.Description = model.Risk.Description;
//            risk.CreateDate = model.Risk.CreateDate;
//            risk.Solution = model.Risk.Solution;
//            risk.Status = model.Risk.Status;
//            risk.RiskType = model.Risk.RiskType;
//            risk.Probability = model.Risk.Probability;
//            risk.Level = model.Risk.Level;
//            risk.Incidence = model.Risk.Incidence;
//            applicationDbContext.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteRisk(int id)
//        {
//            var risk = await applicationDbContext.Risks.FirstOrDefaultAsync(a => a.Id == id);
//            applicationDbContext.Risks.Remove(risk);
//            applicationDbContext.SaveChanges();
//            return RedirectToAction("Index");
//        }

//    }
//}
