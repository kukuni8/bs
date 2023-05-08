using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class MissionEditViewModel
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
        [Display(Name = "开始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        [Display(Name = "完成时间")]
        [DataType(DataType.Date)]
        public DateTime FinishedTime { get; set; }
        [Display(Name = "优先级")]
        public MissionPriority Priority { get; set; }
        [Display(Name = "状态")]
        public MissionStatus Status { get; set; }

        [Display(Name = "所属项目")]
        public Project Project { get; set; }

        public int? PutForwardId { get; set; }
        [Display(Name = "提出人")]
        public ApplicationUser PutForward { get; set; }
        [Display(Name = "执行者")]
        public List<string> Executors { get; set; }
        public List<MissionDialogue> Dialogues { get; set; }

        public string Content { get; set; }
    }
}
