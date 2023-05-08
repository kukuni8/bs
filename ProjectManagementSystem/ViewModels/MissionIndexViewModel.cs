using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class MissionIndexViewModel
    {
        public IEnumerable<Mission> Missions { get; set; }
    }
}
