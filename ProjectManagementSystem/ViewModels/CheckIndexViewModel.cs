using ProjectManagementSystem.Data;

namespace ProjectManagementSystem.ViewModels
{
    public class CheckIndexViewModel
    {
        public IEnumerable<CheckInfoViewModel> MissionChecks { get; set; }
        public IEnumerable<CheckInfoViewModel> DefectChecks { get; set; }
        public IEnumerable<CheckInfoViewModel> RiskChecks { get; set; }

        public IEnumerable<CheckInfoViewModel> UnMissionChecks { get; set; }
        public IEnumerable<CheckInfoViewModel> UnDefectChecks { get; set; }
        public IEnumerable<CheckInfoViewModel> UnRiskChecks { get; set; }
    }

    public class CheckInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CheckStatus Status { get; set; }
        public CheckType CheckType { get; set; }
    }
}
