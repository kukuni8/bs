﻿using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class RiskAddViewModel
    {
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "风险影响")]
        public string Incidence { get; set; }
        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "风险预案")]
        public string Solution { get; set; }
        [Display(Name = "风险等级")]
        public RiskLevel Level { get; set; }
        [Display(Name = "状态")]
        public RiskStatus Status { get; set; }
        [Display(Name = "风险类型")]
        public RiskType RiskType { get; set; }

        public List<ApplicationUser> UsersInProject { get; set; }



        public int FunctionaryId { get; set; }
        [Display(Name = "处理人")]
        [ForeignKey(nameof(FunctionaryId))]
        public ApplicationUser Functionary { get; set; }

        public int PutForwardId { get; set; }
        [Display(Name = "提出人")]
        [ForeignKey(nameof(PutForwardId))]
        public ApplicationUser PutForward { get; set; }


        public int? ProjectId { get; set; }
        [Display(Name = "所属项目")]
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
    }
}
