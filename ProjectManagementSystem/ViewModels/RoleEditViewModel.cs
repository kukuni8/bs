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
        public int RoleId { get; set; }
        [Required]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }

        public List<PriorityInfos> Infos { get; set; } = new List<PriorityInfos>()
        {
            new PriorityInfos(){AboutName="用户管理",priorityInfos=new List<PriorityInfo>{new PriorityInfo {Name="用户添加",IsOn=false },new PriorityInfo {Name="用户编辑",IsOn=false },new PriorityInfo {Name="用户删除",IsOn=false } }},
            new PriorityInfos(){AboutName="角色管理",priorityInfos=new List<PriorityInfo>{new PriorityInfo {Name="角色添加",IsOn=false },new PriorityInfo {Name="角色编辑",IsOn=false },new PriorityInfo {Name="角色删除",IsOn=false } }},
            new PriorityInfos(){AboutName="项目管理",priorityInfos=new List<PriorityInfo>{new PriorityInfo {Name="项目添加",IsOn=false },new PriorityInfo {Name="项目编辑",IsOn=false },new PriorityInfo {Name="项目删除",IsOn=false } }},
            new PriorityInfos(){AboutName="任务管理",priorityInfos=new List<PriorityInfo>{new PriorityInfo {Name="任务添加",IsOn=false },new PriorityInfo {Name="任务编辑",IsOn=false },new PriorityInfo {Name="任务删除",IsOn=false } }},
            new PriorityInfos(){AboutName="风险管理",priorityInfos=new List<PriorityInfo>{new PriorityInfo {Name="风险添加",IsOn=false },new PriorityInfo {Name="风险编辑",IsOn=false },new PriorityInfo {Name="风险删除",IsOn=false } }},
            new PriorityInfos(){AboutName="缺陷管理",priorityInfos=new List<PriorityInfo>{new PriorityInfo {Name="缺陷添加",IsOn=false },new PriorityInfo {Name="缺陷编辑",IsOn=false },new PriorityInfo {Name="缺陷删除",IsOn=false } }},
        };
    }
}
