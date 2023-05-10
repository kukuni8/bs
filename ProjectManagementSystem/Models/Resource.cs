using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Resource
    {
        public int Id { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }
        [Display(Name = "数量")]
        public int Number { get; set; }

        public Project Project { get; set; }

        public string ImagePath { get; set; }

        public List<ResourceChange> Changes { get; set; }

    }

    public class ResourceChange
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public Resource Resource { get; set; }

        public DateTime Time { get; set; }

        public string Description { get; set; }
        public string Reason { get; set; }

        public int Number { get; set; }
    }

}
