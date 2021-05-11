using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Admin
{
    public class AdminCreateLarareViewModel
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
        public int[] KursId { get; set; }
        public ICollection<Kurs> Kurser { get; set; }
      
    }
}
