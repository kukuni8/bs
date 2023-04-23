﻿using ProjectManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementSystem.ViewModels
{
	public class RoleAddViewModel
	{
		[Required]
		[Display(Name = "角色名称")]
		public string RoleName { get; set; }

		public Dictionary<string, bool> AuthorityDic { get; set; } = new Dictionary<string, bool>
		{
			{ "用户管理",false },
			{ "角色管理",false },
			{ "项目管理",false },
			{ "任务管理",false },
			{ "风险管理",false },
			{ "缺陷管理",false },
		};
	}
}
