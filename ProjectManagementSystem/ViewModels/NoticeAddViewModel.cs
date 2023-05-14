namespace ProjectManagementSystem.ViewModels
{
    public class NoticeAddViewModel
    {
        public int ProjectId { get; set; }
        public bool IsInfo { get; set; }
        public bool IsWarning { get; set; }
        public bool IsDanger { get; set; }
        public bool IsSuccess { get; set; }
        public string Information { get; set; }
    }
}
