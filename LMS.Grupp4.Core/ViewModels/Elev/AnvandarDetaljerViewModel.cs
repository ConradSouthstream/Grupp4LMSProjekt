using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Elev
{
  public class AnvandarDetaljerViewModel
    {
        public string Id { get; set; }
        public int KursId { get; set; }
        public string EfterNamn { get; set; }
        public string ForeNamn { get; set; }
        // public string Email { get; set; }
        public string Avatar { get; set; }
        public ICollection<Kurs> Kurser { get; set; }
    }
}
