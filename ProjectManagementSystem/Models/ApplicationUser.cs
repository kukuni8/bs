using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        [Display(Name = "简介")]
        public string About { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "职位")]
        public string Job { get; set; }
        [Display(Name = "姓名")]
        public string TrueName { get; set; }

        [Display(Name = "部门")]
        public string Department { get; set; }

        [Display(Name = "入职时间")]
        [DataType(DataType.Date)]
        public DateTime JobDate { get; set; }

        [Display(Name = "角色")]
        public string RoleName { get; set; }

        public List<Mission> Missions { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}
