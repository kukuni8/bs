using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class MissionIndexViewModel
    {
        public int CurProjectId { get; set; }
        public string CurProjectName { get; set; }
        public ProjectAddMissionViewModel AddMission { get; set; }

        public ProjectEditMissionViewModel EditMission { get; set; }

        // public ProjectDeleteMissionViewModel DeleteMission { get; set; }
        public ProjectMissionIndexViewModel ProjectMissionIndexViewModel { get; set; }
        public IEnumerable<ApplicationUser> UsersInTheProject { get; set; }
    }
}
