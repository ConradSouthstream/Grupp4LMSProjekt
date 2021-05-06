using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Modul
{
    public class ModulCreateViewModel
    {
        public int Id { get; set; }
        public int KursId { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public DateTime SlutDatum { get; set; }
        public DateTime StartDatum { get; set; }





    }
}
