using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
	public class MissionIndexViewModel
	{
		public IEnumerable<Mission> UntreatedMissions { get; set; }
		public IEnumerable<Mission> ProcessOnMissions { get; set; }
		public IEnumerable<Mission> FinishedMissions { get; set; }

		public Mission CurrentMission { get; set; }

		[Display(Name = "名称")]
		public string Name { get; set; }
		[Display(Name = "描述")]
		public string Description { get; set; }

		[Display(Name = "截止日期")]
		public DateTime Deadline { get; set; }
		[Display(Name = "优先级")]
		public string Priority { get; set; }
		[Display(Name = "状态")]
		public string Status { get; set; }
		[Display(Name = "执行者")]
		public string Executor { get; set; }
	}
}
