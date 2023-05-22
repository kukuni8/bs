using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Display(Name = "文档名称")]
        public string Name { get; set; }
        [Display(Name = "文档概要")]
        public string Summary { get; set; }
        [Display(Name = "文档文件")]
        public string BookFile { get; set; }

        public Project Project { get; set; }
    }
}
