using System.Collections.Generic;
using System.ComponentModel;

namespace LMS.Grupp4.Core.ViewModels.Elev
{
    public class ElevDetailsViewModel
    {
        [DisplayName("Elevens id")]
        public string ElevId { get; set; }

        [DisplayName("Förnamn")]
        public string ForNamn { get; set; }

        [DisplayName("Efternamn")]
        public string EfterNamn { get; set; }

        public string Avatar { get; set; }

        [DisplayName("Namn")]
        public string FullNamn => $"{ForNamn} {EfterNamn}";

        //public ICollection<Kurs> Kurser { get; set; }

        /// <summary>
        /// Information om kursen som eleven läser
        /// </summary>
        public KursElevDetailsViewModel Kurs { get; set; }

        /// <summary>
        /// Moduler som ingår i kursen
        /// </summary>
        public ICollection<ModulElevDetailsViewModel> Moduler { get; set; }
    }
}
