using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.DokumentViewModel
{
    public class DocumentInput
    {
        [Required]
        [MinLength(4)]
        public string Namn { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
