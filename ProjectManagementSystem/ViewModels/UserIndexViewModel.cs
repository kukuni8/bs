using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.ViewModels
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "姓名")]
        public string TrueName { get; set; }

        [Display(Name = "年龄")]
        public string Age { get; set; }

        [Display(Name = "部门")]
        public string Department { get; set; }

        [Display(Name = "职位")]
        public string Job { get; set; }

        [Display(Name = "工龄")]
        public string JobYear { get; set; }

        [Display(Name = "角色")]
        public string RoleName { get; set; }
    }
}
