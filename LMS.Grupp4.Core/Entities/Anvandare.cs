using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
    public class Anvandare : IdentityUser
    {
        public string EfterNamn { get; set; }
        public string ForeNamn { get; set; }
        [NotMapped]
        public string FullNamn => $"{ForeNamn} {EfterNamn}";
        public string Mejl { get; set; }
        public string Losenord { get; set; }
        public ICollection<AnvandareKurs> Kurser { get; set; }
        public ICollection<Dokument> Dokumenter { get; set; }


    }
}
