using ProjectManagementSystem.Data;

namespace ProjectManagementSystem.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateTime { get; set; }
        public NoticeType NoticeType { get; set; }
        public string Information { get; set; }
        public ApplicationUser Putforward { get; set; }
        public List<NoticeReceiver> Receivers { get; set; }
        public Project Project { get; set; }
    }
    public class NoticeDTO
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateTime { get; set; }
        public string NoticeType { get; set; }
        public string Information { get; set; }
        public string Putforward { get; set; }
    }
}
