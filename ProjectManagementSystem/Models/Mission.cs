using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManagementSystem.Models
{
    public class Mission
    {
        [Display(Name = "编号")]
        public int Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "类型")]
        public MissionType Type { get; set; }

        [Display(Name = "创建日期")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "更新日期")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [ForeignKey(nameof(Priority))]
        public int PriorityId { get; set; }
        [Display(Name = "优先级")]
        public MissionPriority Priority { get; set; }

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }
        [Display(Name = "状态")]
        public MissionStatus Status { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        [Display(Name = "所属项目")]
        public Project Project { get; set; }

        [Display(Name = "执行者")]
        public List<ApplicationUser> Executor { get; set; }



        public List<MissionDialogue> Dialogues { get; set; }

    }
    public class MissionStatus
    {
        public int Id { get; set; }
        [Display(Name = "任务状态")]
        public string Status { get; set; }

        public List<Mission> Missions { get; set; }
    }
    public class MissionPriority
    {
        public int Id { get; set; }
        [Display(Name = "优先级类型")]
        public string Priority { get; set; }
        public List<Mission> Missions { get; set; }
    }

    public class MissionType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public List<Mission> Missions { get; set; }
    }
    public class MissionDialogue
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        public Mission Mission { get; set; }
    }
}
