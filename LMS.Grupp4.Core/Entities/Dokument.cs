using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
  public  class Dokument
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public DateTime UppladdningsDatum { get; set; }

        //Navigation property 
        public virtual Anvandare Anvandare { get; set; }
        public virtual Kurs Kurs { get; set; }
        public int? KursId { get; set; }

        public virtual Modul Modul { get; set; }
        public int? ModulId { get; set; }
        public virtual Aktivitet  Aktivitet { get; set; }
        public int? AktivitetId { get; set; }


    }
}
