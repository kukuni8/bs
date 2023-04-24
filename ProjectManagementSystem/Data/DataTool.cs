namespace ProjectManagementSystem.Data
{
	public class DataTool
	{
		public Dictionary<string, bool> Prioritys = new Dictionary<string, bool>()
		{
			 { "用户添加",false },
			 { "用户编辑",false },
			 { "用户删除",false },

			 { "角色添加",false },
			 { "角色编辑",false },
			 { "角色删除",false },

			 { "项目添加",false },
			 { "项目编辑",false },
			 { "项目删除",false },

			 { "任务添加",false },
			 { "任务编辑",false },
			 { "任务删除",false },

			 { "风险添加",false },
			 { "风险编辑",false },
			 { "风险删除",false },

			 { "缺陷添加",false },
			 { "缺陷编辑",false },
			 { "缺陷删除",false },
		};
	}
}
