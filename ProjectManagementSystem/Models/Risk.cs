﻿using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Risk
    {
        [Display(Name = "编号")]
        public int Id { get; set; }

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

        public int? FunctionaryId { get; set; }
        [Display(Name = "负责人")]
        public ApplicationUser Functionary { get; set; }


        public int? PutForwardId { get; set; }
        [Display(Name = "提出人")]
        public ApplicationUser PutForward { get; set; }

        [Display(Name = "所属项目")]
        public Project Project { get; set; }
    }
}
