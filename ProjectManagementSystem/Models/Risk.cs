using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Risk
    {
        [Display(Name = "编号")]
        public int Id { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "发生概率")]
        public string Probability { get; set; }
        [Display(Name = "风险影响")]
        public string Incidence { get; set; }
        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "风险预案")]
        public string Solution { get; set; }
        [Display(Name = "风险等级")]
        public string Level { get; set; }
        [Display(Name = "状态")]
        public string Status { get; set; }
        [Display(Name = "风险类型")]
        public string RiskType { get; set; }
    }

    public class RiskLevel
    {
        public int Id { get; set; }
        [Display(Name = "风险等级")]
        public string LevelName { get; set; }
    }
    public class RiskStatus
    {
        public int Id { get; set; }
        [Display(Name = "风险状态")]
        public string StatusName { get; set; }
    }
    public class RiskType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
}
