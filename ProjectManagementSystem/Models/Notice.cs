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
        public ApplicationUser ApplicationUser { get; set; }
    }
}
