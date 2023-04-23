using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.ViewModels
{
	public class AccountIndexViewModal
	{
		public ApplicationUser ApplicationUser { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }

		public string SelectedTab { get; set; }
	}
}
