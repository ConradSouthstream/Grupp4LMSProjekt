using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
    public class DocumentInput
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
