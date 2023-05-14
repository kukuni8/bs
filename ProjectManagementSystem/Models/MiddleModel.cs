﻿namespace ProjectManagementSystem.Models
{
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class MissionExecutor
    {
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int MissionId { get; set; }
        public Mission Mission { get; set; }
    }

    public class NoticeReceiver
    {
        public int NoticeId { get; set; }
        public Notice Notice { get; set; }

        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

    }
}
