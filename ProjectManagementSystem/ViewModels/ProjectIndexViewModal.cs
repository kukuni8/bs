using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class ProjectIndexViewModal
    {
        public IEnumerable<Project> projects { get; set; }
        public Project HelpProject { get; set; }
    }
}
