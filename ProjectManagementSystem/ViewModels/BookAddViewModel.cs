using ProjectManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagementSystem.ViewModels
{
    public class BookAddViewModel
    {
        public int Id { get; set; }
        [Display(Name = "文档名称")]
        public string Name { get; set; }
        [Display(Name = "文档概要")]
        public string Summary { get; set; }
        [Display(Name = "文档文件")]
        public IFormFile BookFile { get; set; }

        public int ProjectId { get; set; }
    }
}
