using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.ViewModels
{
    public class ProjectIndexViewModal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "已完成")]
        public float FinishedPercent { get; set; }
        [Display(Name = "进行中")]
        public float InProgressPercent { get; set; }
        [Display(Name = "已超时")]
        public float TimeOutPercent { get; set; }
        [Display(Name = "待处理")]
        public float UnDealPercent { get; set; }
        [Display(Name = "截止日期")]
        public DateTime Deadline { get; set; }
        [Display(Name = "创建日期")]
        public DateTime CreateTime { get; set; }
    }


}
