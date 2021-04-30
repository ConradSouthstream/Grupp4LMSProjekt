using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Anvandar entitet
    /// </summary>
    public class Anvandare : IdentityUser
    {
        //Ärver från IdentityUserProperties
        public string EfterNamn { get; set; }
        public string ForeNamn { get; set; }
        public string Avatar { get; set; }

        public ICollection<AnvandareKurs> Kurser { get; set; }
        public ICollection<Dokument> Dokument { get; set; }

        ///// <summary>
        ///// Användarens namn
        ///// </summary>
        //[Required(ErrorMessage = "Ni måste ge användaren ett namn")]
        //[StringLength(50, ErrorMessage = "Max längd är 50 tecken")]
        //public string AnvandarNamn { get; set; }

        ///// <summary>
        ///// Användarens epost
        ///// </summary>
        //[Required(ErrorMessage = "Ni måste skriva en epost")]
        //[StringLength(50, ErrorMessage = "Max längd är 50 tecken")]
        //public string Epost { get; set; }

        ///// <summary>
        ///// Användarens lösenord
        ///// </summary>
        //[Required(ErrorMessage = "Ni måste skriva ett lösenord")]
        //[StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        //public string PassWord { get; set; }
    }
}
