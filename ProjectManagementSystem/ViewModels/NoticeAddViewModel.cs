using ProjectManagementSystem.Data;

namespace ProjectManagementSystem.ViewModels
{
    public class NoticeAddViewModel
    {
        public int ProjectId { get; set; }
        public NoticeType NoticeType { get; set; }
        public string Information { get; set; }
    }

}
