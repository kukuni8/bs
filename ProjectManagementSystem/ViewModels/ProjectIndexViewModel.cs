using ProjectManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
	public class ProjectIndexViewModel
	{
		[Required]
		[Display(Name = "编号")]
		public int Id { get; set; }
		[Required]
		[Display(Name = "名称")]
		public string Name { get; set; }
		[Required]
		[Display(Name = "描述")]
		public string Description { get; set; }
		[Required]
		[Display(Name = "创建日期")]
		public DateTime CreatedDate { get; set; }
		[Required]
		[Display(Name = "截止日期")]
		[DataType(DataType.Date)]
		public DateTime Deadline { get; set; }

		public ProjectStatus Status { get; set; }
		[Required]
		[Display(Name = "负责人")]
		public string FunctionaryId { get; set; }
		[Display(Name = "提出人")]
		public string PutForwardId { get; set; }

		public int FinishedPercent { get; set; }

		public int InProgressPercent { get; set; }

		public int UnDealPercent { get; set; }
	}
}
