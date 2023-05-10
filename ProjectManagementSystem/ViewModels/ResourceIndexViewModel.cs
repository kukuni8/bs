using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class ResourceIndexViewModel
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }
        [Display(Name = "数量")]
        public int Number { get; set; }

        public string ImagePath { get; set; }

        public int ProjectId { get; set; }

        public List<ResourceChange> ResourceChanges { get; set; }
    }
}
