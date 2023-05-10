using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class ResourceAddViewModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "图片")]
        public IFormFile Image { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }
        [Display(Name = "数量")]
        public int Number { get; set; }
    }
}
