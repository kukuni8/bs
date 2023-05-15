namespace ProjectManagementSystem.ViewModels
{
    public class RiskIndexViewModel
    {
        public int CurProjectId { get; set; }
        public string CurProjectName { get; set; }
        public IEnumerable<RiskEditViewModel> UnCheckRiskViewModels { get; set; }
        public IEnumerable<RiskEditViewModel> UnDealRiskViewModels { get; set; }
        public IEnumerable<RiskEditViewModel> SettledRiskViewModels { get; set; }
        public IEnumerable<RiskEditViewModel> DiscardedRiskViewModels { get; set; }
    }
}
