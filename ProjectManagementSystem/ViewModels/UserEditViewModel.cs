using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementSystem.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "邮箱")]
        public string Email { get; set; }


        [Display(Name = "出生日期")]
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

        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
    }
}
