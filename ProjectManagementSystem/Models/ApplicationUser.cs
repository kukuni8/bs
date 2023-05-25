using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using ProjectManagementSystem.Controllers;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class ApplicationUser : IdentityUser<int>
    {

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        [Display(Name = "简介")]
        public string About { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "职位")]
        public Position Job { get; set; }
        [Display(Name = "姓名")]
        public string TrueName { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "部门")]
        public Department Department { get; set; }

        [Display(Name = "入职时间")]
        [DataType(DataType.Date)]
        public DateTime JobDate { get; set; }

        [Display(Name = "角色")]
        public string RoleName { get; set; }

        // 与Project的关系
        public List<ProjectUser> ProjectUsers { get; set; }
        public List<Project> PutForwardProjects { get; set; }
        public List<Project> FunctionaryProjects { get; set; }
        // 与Mission的关系
        public List<Mission> PutForwardMissions { get; set; }
        public List<MissionExecutor> MissionExecutors { get; set; }
        // 与Defect的关系
        public List<Defect> PutForwardDefects { get; set; }
        public List<Defect> FunctionaryDefects { get; set; }
        // 与Risk的关系
        public List<Risk> PutForwardRisks { get; set; }
        public List<Risk> FunctionaryRisks { get; set; }
        // 与MissionDialogue的关系
        public List<MissionDialogue> MissionDialogues { get; set; }
        public List<Notice> Notices { get; set; }
        public List<ResourceChange> ResourceChanges { get; set; }
        public List<FundChange> FundChanges { get; set; }
        public List<NoticeReceiver> NoticeReceived { get; set; }
        public List<ChatRecord> PutforwardChats { get; set; }
        public List<ChatRecord> ReceiveChats { get; set; }
        public override string ToString()
        {
            return UserName;
        }
    }
}
