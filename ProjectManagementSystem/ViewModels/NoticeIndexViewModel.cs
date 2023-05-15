using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class NoticeIndexViewModel
    {
        public IEnumerable<Notice> Notices { get; set; }

        public NoticeAddViewModel NoticeAddViewModel { get; set; }
    }
}
