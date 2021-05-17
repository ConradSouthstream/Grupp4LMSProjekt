using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LMS.Grupp4.Core.Entities;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Anvandar entitet
    /// </summary>
    public class Anvandare : IdentityUser
    {

        //Ärver från IdentityUserProperties
        public string EfterNamn { get; set; }
        public string ForNamn { get; set; }
        public string Avatar { get; set; }
        public string FullNamn => $"{ForNamn} {EfterNamn}";

        public ICollection<AnvandareKurs> KurserAnvandare { get; set; }
        public ICollection<Dokument> Dokument { get; set; }
        //Många till Många
        public List<Kurs> Kurser { get; set; }

        [NotMapped]
        public bool IsLarare { get; set; } = false;



    }
}
