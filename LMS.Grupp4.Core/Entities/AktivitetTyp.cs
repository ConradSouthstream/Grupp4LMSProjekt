using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
  public  class AktivitetTyp
    {
        public int Id { get; set; }
        public string Namn { get; set; }


        //Navigation property
        public Aktivitet Aktivitet { get; set; }
        public int AktivitetId { get; set; }
    }
}
