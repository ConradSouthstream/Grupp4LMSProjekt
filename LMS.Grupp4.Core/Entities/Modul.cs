using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
   public  class Modul
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }
        public string Beskrivning{ get; set; }

        //Navigation property
        public Kurs Kurs { get; set; }
        public int KursId { get; set; }
        public ICollection<Aktivitet> Aktiviteter { get; set; }
        public ICollection<Dokument> Dokument { get; set; }

    }
}
