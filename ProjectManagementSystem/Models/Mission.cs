using ProjectManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Linq;

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
        [Display(Name = "创建日期")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        [Display(Name = "更新日期")]
        [DataType(DataType.Date)]
        public DateTime UpdateDate { get; set; }
        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        [Display(Name = "优先级")]
        public MissionPriority Priority { get; set; }
        [Display(Name = "状态")]
        public MissionStatus Status { get; set; }

        [Display(Name = "所属项目")]
        public Project Project { get; set; }

        public int? PutForwardId { get; set; }
        public ApplicationUser PutForward { get; set; }

        public List<MissionExecutor> MissionExecutors { get; set; }
        public List<MissionDialogue> Dialogues { get; set; }

    }
    public class MissionDialogue
    {
        public int Id { get; set; }

        // 添加与ApplicationUser的关系
        public int UserId { get; set; }
        public ApplicationUser Speaker { get; set; }

        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        public int MissionId { get; set; }
        public Mission Mission { get; set; }
    }
}
