using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
  public  class Kurs
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }
        public string Beskrivning { get; set; }


        //Navigation prope
        public ICollection<Dokument> Dokument { get; set; }
        public ICollection<Modul> Moduler { get; set; }
        public  ICollection<AnvandareKurs> Anvandare { get; set; }


    }
}
