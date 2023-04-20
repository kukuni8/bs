using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(18)]
        public string IdCardNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
