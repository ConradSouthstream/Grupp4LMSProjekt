﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.Entities
{
    public class Kurs
    {
        public int Id { get; set; }

        [DisplayName("Aktivitetens namn")]
        public string Namn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDatum { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SlutDatum { get; set; }
        public string Beskrivning { get; set; }
        //Navigation prope
        public ICollection<Dokument> Dokument { get; set; }
        public ICollection<Modul> Moduler { get; set; }
        [Display(Name = "Inskrivna Elever")]
        public ICollection<AnvandareKurs> AnvandareKurser { get; set; }

        //Många till många
        public ICollection<Anvandare> Anvandare { get; set; }

        [Required]
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
