using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Aktivitet
{
    public class AktivitetListViewModel
    {
        /// <summary>
        /// Primär nyckel. Id
        /// </summary>
        public int AktivitetId { get; set; }

        /// <summary>
        /// Aktivitetens namn
        /// </summary>
        [DisplayName("Aktivitetens namn")]
        public string AktivitetNamn { get; set; }

        /// <summary>
        /// Tid när aktiviteten starta
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Starttid")]
        public DateTime StartTid { get; set; }

        /// <summary>
        /// Tid när aktiviteten slutar
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Sluttid")]
        public DateTime SlutTid { get; set; }

        /// <summary>
        /// Beskrivning av aktiviteten
        /// </summary>
        [DisplayName("Beskrivning")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Typ av aktivitet
        /// </summary>
        [DisplayName("Aktivitetstyp")]
        public string AktivitetTypNamn { get; set; }

        /// <summary>
        /// Modulens namn
        /// </summary>
        [DisplayName("Modulnamn")]
        public string ModulNamn { get; set; }

        /// <summary>
        /// Modulens starttid
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Modulens starttid")]
        public DateTime ModulStartTid { get; set; }

        /// <summary>
        /// Modulens sluttid
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Modulens sluttid")]
        public DateTime ModulSlutTid { get; set; }
    }
}
