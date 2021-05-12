using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.ViewModels.Aktivitet
{
    public class AktivitetListViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Aktivitetens namn
        /// </summary>
        [DisplayName("Aktivitetnamn")]
        public string Namn { get; set; }

        /// <summary>
        /// Datum när aktiviteten starta
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Startdatum")]
        public DateTime StartDatum { get; set; }

        /// <summary>
        /// Datum när aktiviteten slutar
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Slutdatum")]
        public DateTime SlutDatum { get; set; }

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

        [DisplayName("Modulens id")]
        public int ModulId { get; set; }

        /// <summary>
        /// Modulens namn
        /// </summary>
        [DisplayName("Modulnamn")]
        public string ModulNamn { get; set; }

        /// <summary>
        /// Modulens startdatum
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Modulens startdatum")]
        public DateTime ModulStartDatum { get; set; }

        /// <summary>
        /// Modulens slutdatum
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Modulens slutdatum")]
        public DateTime ModulSlutDatum { get; set; }
        public int KursId { get; set; }

        [DisplayName("Kursnamn")]
        public string KursNamn { get; set; }
        public int MyProperty { get; set; }
        public ICollection<Dokument> Dokument { get; set; }

    }
}
