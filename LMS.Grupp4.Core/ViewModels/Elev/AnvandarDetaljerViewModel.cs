using LMS.Grupp4.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace LMS.Grupp4.Core.ViewModels.Elev
{
    public class AnvandarDetaljerViewModel
    {
        [DisplayName("Användarens id")]
        public string Id { get; set; }

        [DisplayName("Kursens id")]
        public int KursId { get; set; }

        [DisplayName("Efternamn")]
        public string EfterNamn { get; set; }

        [DisplayName("Förnamn")]
        public string ForNamn { get; set; }

        [DisplayName("Epost")]
        public string Email { get; set; }

        public string Avatar { get; set; }
        public ICollection<Kurs> Kurser { get; set; }

        [DisplayName("Namn")]
        public string FullNamn => $"{ForNamn} {EfterNamn}";

        [DisplayName("Lärare")]
        public bool IsLarare { get; set; }
    }
}
