﻿using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
	public class DefectEditViewModel
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

		public string FunctionaryId { get; set; }
		[ForeignKey(nameof(FunctionaryId))]
		[Display(Name = "负责人")]
		public ApplicationUser Functionary { get; set; }

		public string PutForwardId { get; set; }
		[ForeignKey(nameof(PutForwardId))]
		[Display(Name = "提出人")]
		public ApplicationUser PutForward { get; set; }

		public int? ProjectId { get; set; }
		[Display(Name = "所属项目")]
		[ForeignKey(nameof(ProjectId))]
		public Project Project { get; set; }
	}
}
