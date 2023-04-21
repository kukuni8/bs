namespace ProjectManagementSystem.Models
{
	public class Dialogue
	{
		public int Id { get; set; }
		public ApplicationUser User { get; set; }
		public string Content { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
