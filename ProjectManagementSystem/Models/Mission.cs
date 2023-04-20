using System.ComponentModel.DataAnnotations;

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
        public string Type { get; set; }
        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "更新日期")]
        public DateTime UpdateDate { get; set; }
        [Display(Name = "截止日期")]
        public DateTime Deadline { get; set; }
        [Display(Name = "优先级")]
        public MissionPriority Priority { get; set; }
        [Display(Name = "状态")]
        public string Status { get; set; }
        [Display(Name = "执行者")]
        public string Executor { get; set; }
    }
    public class MissionStatus
    {
        public int Id { get; set; }
        [Display(Name = "任务状态")]
        public string Status { get; set; }
    }
    public class MissionPriority
    {
        public int Id { get; set; }
        [Display(Name = "优先级类型")]
        public string Priority { get; set; }
    }
}
