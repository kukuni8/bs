using ProjectManagementSystem.Data;

namespace ProjectManagementSystem.Models
{
    public class Fund
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int ProjectId { get; set; } // 外键属性
        public Project Project { get; set; }
        public List<FundChange> Changes { get; set; }
    }

    public class FundChange
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public FundChangeType ChangeType { get; set; }

        public Fund Fund { get; set; }
        public ApplicationUser User { get; set; }
    }
}
