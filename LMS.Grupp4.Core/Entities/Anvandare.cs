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

        //Ärver från IdentityUserProperties
        public string EfterNamn { get; set; }
        public string ForeNamn { get; set; }
        public string Avatar { get; set; }
        public string FullNamn => $"{ForeNamn} {EfterNamn}";

        public ICollection<AnvandareKurs> KurserAnvandare { get; set; }
        public ICollection<Dokument> Dokument { get; set; }


    }
}
