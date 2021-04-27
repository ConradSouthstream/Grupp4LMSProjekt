using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
  public  class Aktivitet
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }
        public string Beskrivning { get; set; }
        public int ModulId { get; set; }
        public int AktivitetTypId { get; set; }

        //Navigation property       
        public Modul Modul { get; set; }
        public AktivitetTyp AktivitetTyp { get; set; }




    }
}
