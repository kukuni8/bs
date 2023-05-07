namespace ProjectManagementSystem.Models
{
    public class ProjectUser
    {
        public int PId { get; set; }
        public Project Project { get; set; }

        public int UId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class MissionExecutor
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int MissionId { get; set; }
        public Mission Mission { get; set; }
    }

}
