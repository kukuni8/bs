using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class BookEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "文档名称")]
        public string Name { get; set; }
        [Display(Name = "文档概要")]
        public string Summary { get; set; }
        [Display(Name = "文档内容")]
        public string Content { get; set; }
        [Display(Name = "文档封面")]
        public IFormFile CoverImage { get; set; }
        [Display(Name = "文档封面路径")]
        public string CoverImagePath { get; set; }

        public int ProjectId { get; set; }
    }
}
