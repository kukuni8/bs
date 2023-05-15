namespace ProjectManagementSystem.ViewModels
{
    public class UserViewModel
    {
        public int CurProjectId { get; set; }
        public IEnumerable<ProjectUserIndexViewModel> ProjectUserIndexViewModels { get; set; }

        public List<ProjectUserNotInProjectModel> UsersNotInThisProject { get; set; }
    }
}
