using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Defect
    {
        [Display(Name = "编号")]
        public int Id { get; set; }

        [Display(Name = "名字")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "解决方案")]
        public string Solution { get; set; }
        [Display(Name = "缺陷类型")]
        public DefectType Type { get; set; }

        [Display(Name = "负责人")]
        public ApplicationUser Functionary { get; set; }
        [Display(Name = "提出人")]
        public ApplicationUser PutForward { get; set; }
        [Display(Name = "状态")]
        public DefectStatus Status { get; set; }
    }
    public class DefectType
    {
        public int Id { get; set; }
        [Display(Name = "缺陷类型")]
        public string TypeName { get; set; }

        public List<Defect> Defects { get; set; }
    }
    public class DefectStatus
    {
        public int Id { get; set; }
        [Display(Name = "缺陷状态")]
        public string StatusName { get; set; }
        public List<Defect> Defects { get; set; }
    }

}