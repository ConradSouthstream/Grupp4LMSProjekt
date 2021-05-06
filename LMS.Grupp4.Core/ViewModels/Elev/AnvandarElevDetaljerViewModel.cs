using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Elev
{
    public class AnvandarElevDetaljerViewModel
    {
        /// <summary>
        /// Kursens namn
        /// </summary>
        [DisplayName("Kursnamn")]
        public string KursNamn { get; set; }

        /// <summary>
        /// Elever
        /// </summary>
        public List<AnvandarDetaljerViewModel> Elever { get; set; }

        /// <summary>
        /// Lärare
        /// </summary>
        public List<AnvandarDetaljerViewModel> Larare { get; set; }
    }
}
