using ProjectManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

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
        public ProjectStatus Status { get; set; }
        [Required]
        [Display(Name = "预算")]
        public double Budget { get; set; }

        [Display(Name = "目的")]
        public string Target { get; set; }

        [Required]
        [Display(Name = "负责人")]
        public string FunctionaryId { get; set; }
        [ForeignKey(nameof(FunctionaryId))]
        public ApplicationUser Functionary { get; set; }
        [Display(Name = "提出人")]
        public string PutForwardId { get; set; }
        [ForeignKey(nameof(PutForwardId))]
        public ApplicationUser PutForward { get; set; }


        public List<Mission> Missions { get; set; }

        public List<Risk> Risks { get; set; }

        public List<Defect> Defects { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

}
