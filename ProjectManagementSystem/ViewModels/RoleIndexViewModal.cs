namespace ProjectManagementSystem.ViewModels
{
    public class RoleIndexViewModal
    {
        public List<RoleInfo> Roles { get; set; }
    }

    public class RoleInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
    }
}
