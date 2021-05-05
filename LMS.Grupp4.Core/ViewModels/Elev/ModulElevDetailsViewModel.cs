using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.ViewModels.Elev
{
    public class ModulElevDetailsViewModel
    {
        [DisplayName("Modulens id")]
        public int ModulId { get; set; }

        /// <summary>
        /// Modulens namn
        /// </summary>
        [DisplayName("Namn")]
        public string Namn { get; set; }

        /// <summary>
        /// Datum när modulen starta
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Startdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDatum { get; set; }

        /// <summary>
        /// Datum när modulen slutar
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Slutdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SlutDatum { get; set; }

        /// <summary>
        /// Beskrivning av modulen
        /// </summary>
        [DisplayName("Beskrivning")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Kursensnamn
        /// </summary>
        [DisplayName("Kursensnamn")]
        public string KursNamn { get; set; }

        private int ModulStatusId { get; set; }

        [EnumDataType(typeof(Status))]
        [Display(Name = "Status")]
        public Status ModulStatus
        {
            get
            {
                return (Status)this.ModulStatusId;
            }
            set
            {
                this.ModulStatusId = (int)value;
            }
        }

        /// <summary>
        /// Aktivitetet som ingår i modulen
        /// </summary>
        public ICollection<AktivitetElevDetailsViewModel> Aktiviteter { get; set; }
    }
}
