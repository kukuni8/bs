namespace ProjectManagementSystem.ViewModels
{
    public class LookIndexViewModel
    {
        public int ProjectId { get; set; }
        public List<string> UserNames { get; set; }

        public List<string> TrueUserNames { get; set; }

        public int MissionCount { get; set; }
        public decimal Money { get; set; }
        public int UserCount { get; set; }
    }
}
