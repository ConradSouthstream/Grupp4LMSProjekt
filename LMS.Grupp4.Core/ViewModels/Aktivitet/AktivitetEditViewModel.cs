using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.ViewModels.Aktivitet
{
    public class AktivitetEditViewModel
    {
        /// <summary>
        /// Primärnyckel. Id
        /// </summary>
        public int AktivitetId { get; set; }

        /// <summary>
        /// Aktivitetens namn
        /// </summary>
        [Required(ErrorMessage ="Ni måste ange namn på aktiviteten")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        [DisplayName("Aktivitetens namn")]
        public string AktivitetNamn { get; set; }

        /// <summary>
        /// Tid när aktiviteten starta
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en starttid för aktiviteten")]
        [DataType(DataType.Date)]
        [DisplayName("Starttid")]
        [Remote(action: "IfValidDatesEditStartTid", controller: "AktivitetAsync", AdditionalFields = "SlutTid, ModulId, AktivitetId")]
        public DateTime StartTid { get; set; }

        /// <summary>
        /// Tid när aktiviteten slutar
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en sluttid för aktiviteten")]
        [DataType(DataType.Date)]
        [DisplayName("Sluttid")]
        [Remote(action: "IfValidDatesEditSlutTid", controller: "AktivitetAsync", AdditionalFields = "StartTid, ModulId, AktivitetId")]
        public DateTime SlutTid { get; set; }

        /// <summary>
        /// Beskrivning av aktiviteten
        /// </summary>
        [Required(ErrorMessage = "Ni måste ha en beskrivning av aktiviteten")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        [DisplayName("Beskrivning")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Aktivitetens typ
        /// </summary>
        [Required(ErrorMessage = "Ni måste välja en typ av aktivitet")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Ni måste välja en typ av aktivitet")]
        public int AktivitetTypId { get; set; }

        /// <summary>
        /// Modul som aktiviteten tillhör
        /// </summary>
        public int ModulId { get; set; }

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


        // Dropdown

        /// <summary>
        /// AktivitetTyp för dropdown
        /// </summary>
        public List<SelectListItem> AktivitetTyper { get; set; }
    }
}
