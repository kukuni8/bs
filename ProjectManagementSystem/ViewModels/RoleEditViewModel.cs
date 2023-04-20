using ProjectManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementSystem.ViewModels
{
    public class RoleEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }


        public Dictionary<string, bool> AuthorityDic { get; set; } = new Dictionary<string, bool>
        {
            { "用户管理",false },
             { "角色管理",false },
        };
    }
}
