using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class FundIndexViewModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int ProjectId { get; set; } // 外键属性
        public Project Project { get; set; }
        public List<FundChange> Changes { get; set; }

        public decimal Paid { get; set; }

        public decimal Income { get; set; }
    }
}
