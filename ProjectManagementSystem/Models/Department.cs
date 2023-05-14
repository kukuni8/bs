namespace ProjectManagementSystem.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
