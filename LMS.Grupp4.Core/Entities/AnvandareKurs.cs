using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
    public class AnvandareKurs
    {
        public int Id { get; set; }
        public int Betyg { get; set; }
        public int KursId { get; set; }
        public string AnvandareId { get; set; }

        //NavigationPropp
        public Anvandare Anvandare { get; set; }
        public Kurs Kurs { get; set; }
    }
}
