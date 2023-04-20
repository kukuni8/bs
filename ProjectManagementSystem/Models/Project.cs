using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Project
    {
        [Required]
        [Display(Name = "编号")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "描述")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "创建日期")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Display(Name = "更新日期")]
        public DateTime UpdatedDate { get; set; }
        [Required]
        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        [Required]
        [Display(Name = "状态")]
        public string Status { get; set; }
        [Required]
        [Display(Name = "预算")]
        public double Budget { get; set; }
        [Required]
        [Display(Name = "负责人")]
        public string Functionary { get; set; }
        [Display(Name = "提出人")]
        public string PutForward { get; set; }
    }

    public class ProjectStatus
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "项目状态")]
        public string StatusName { get; set; }
    }
}
