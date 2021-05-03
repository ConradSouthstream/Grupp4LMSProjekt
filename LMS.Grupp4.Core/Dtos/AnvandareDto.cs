using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Dtos
{
    public class AnvandareDto
    {
       
        [Required]
        public string EfterNamn { get; set; }
        [Required]
        public string ForNamn { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        public string Avatar { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
