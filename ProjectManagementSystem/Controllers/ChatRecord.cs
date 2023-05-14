using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class ChatRecord
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        public int PutforwardId { get; set; }
        public ApplicationUser Putforward { get; set; }
        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}
