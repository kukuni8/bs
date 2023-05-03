﻿using ProjectManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Defect
    {
        [Display(Name = "编号")]
        public int Id { get; set; }

        [Display(Name = "名字")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "创建日期")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "解决方案")]
        public string Solution { get; set; }
        [Display(Name = "缺陷类型")]
        public DefectType Type { get; set; }

        [Display(Name = "状态")]
        public DefectStatus Status { get; set; }
        [Required]
        [Display(Name = "负责人")]
        public string FunctionaryId { get; set; }
        [ForeignKey(nameof(FunctionaryId))]
        public ApplicationUser Functionary { get; set; }
        [Display(Name = "提出人")]
        public string PutForwardId { get; set; }
        [ForeignKey(nameof(PutForwardId))]
        public ApplicationUser PutForward { get; set; }

        public int? ProjectId { get; set; }
        [Display(Name = "所属项目")]
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
    }


}