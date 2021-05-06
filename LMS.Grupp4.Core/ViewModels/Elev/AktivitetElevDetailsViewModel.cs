using LMS.Grupp4.Core.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.ViewModels.Elev
{
    public class AktivitetElevDetailsViewModel
    {
        /// <summary>
        /// Aktivitetens Id
        /// </summary>
        [DisplayName("Aktivitetens id")]
        public int AktivitetId { get; set; }

        /// <summary>
        /// Aktivitetens namn
        /// </summary>
        [DisplayName("Aktivitetnamn")]
        public string Namn { get; set; }

        /// <summary>
        /// Tid när aktiviteten starta
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Startdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDatum { get; set; }

        /// <summary>
        /// Tid när aktiviteten slutar
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SlutDatum { get; set; }

        /// <summary>
        /// Beskrivning av aktiviteten
        /// </summary>
        [DisplayName("Beskrivning")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Aktivitetens typ
        /// </summary>
        public int AktivitetTypId { get; set; }

        public AktivitetTyp AktivitetTyp { get; set; }

        /// <summary>
        /// Namn på aktiviteten
        /// </summary>
        [DisplayName("Aktivitettyp")]
        public string AktivitetTypNamn { get; set; }

        private int AktivitetStatusId { get; set; }

        [EnumDataType(typeof(Status))]
        [Display(Name = "Status")]
        public Status AktivitetStatus
        {
            get
            {
                return (Status)this.AktivitetStatusId;
            }
            set
            {
                this.AktivitetStatusId = (int)value;
            }
        }

        public int ModulId { get; set; }

        [DisplayName("Modulnamn")]
        public string ModulNamn { get; set; }

        /// <summary>
        /// Kursensnamn
        /// </summary>
        [DisplayName("Kursnamn")]
        public string KursNamn { get; set; }
    }
}
