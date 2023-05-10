using ProjectManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ProjectManagementSystem.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }

        public double Budget { get; set; }

        public int PutForwardId { get; set; }
        [ForeignKey(nameof(PutForwardId))]
        public ApplicationUser PutForward { get; set; }

        public int FunctionaryId { get; set; }
        [ForeignKey(nameof(FunctionaryId))]
        public ApplicationUser Functionary { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }

        public List<Mission> Missions { get; set; }
        public List<Defect> Defects { get; set; }
        public List<Risk> Risks { get; set; }
        public List<Book> Books { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }


}
