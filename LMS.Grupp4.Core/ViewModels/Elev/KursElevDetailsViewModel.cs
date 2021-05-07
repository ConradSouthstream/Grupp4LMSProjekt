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
    public class KursElevDetailsViewModel
    {
        [DisplayName("Kursens id")]
        public int KursId { get; set; }

        [DisplayName("Aktivitetnamn")]
        public string Namn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Startdatum")]
        public DateTime StartDatum { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("Slutdatum")]
        public DateTime SlutDatum { get; set; }

        [DisplayName("Beskrivning")]
        public string Beskrivning { get; set; }

        public int KursStatusId { get; set; }

        [EnumDataType(typeof(Status))]
        [Display(Name = "Status")]
        public Status KursStatus
        {
            get
            {
                return (Status)this.KursStatusId;
            }
            set
            {
                this.KursStatusId = (int)value;
            }
        }
    }
}
