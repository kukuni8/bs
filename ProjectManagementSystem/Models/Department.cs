namespace ProjectManagementSystem.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
