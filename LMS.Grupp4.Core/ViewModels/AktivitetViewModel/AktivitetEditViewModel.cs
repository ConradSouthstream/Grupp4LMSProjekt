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
        public int Id { get; set; }

        /// <summary>
        /// Aktivitetens namn
        /// </summary>
        [Required(ErrorMessage ="Ni måste ange namn på aktiviteten")]
        [DisplayName("Aktivitetnamn")]
        public string Namn { get; set; }

        /// <summary>
        /// Datum när aktiviteten starta
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en startdatum för aktiviteten")]
        [DataType(DataType.Date)]
        [DisplayName("Startdatum")]
        [Remote(action: "IfValidDatesEditStartDatum", controller: "AktivitetAsync", AdditionalFields = "SlutDatum, ModulId, Id")]
        public DateTime StartDatum { get; set; }

        /// <summary>
        /// Datum när aktiviteten slutar
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en slutdatum för aktiviteten")]
        [DataType(DataType.Date)]
        [DisplayName("Slutdatum")]
        [Remote(action: "IfValidDatesEditSlutDatum", controller: "AktivitetAsync", AdditionalFields = "StartDatum, ModulId, Id")]
        public DateTime SlutDatum { get; set; }

        /// <summary>
        /// Beskrivning av aktiviteten
        /// </summary>
        [Required(ErrorMessage = "Ni måste ha en beskrivning av aktiviteten")]
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

        public List<Entities.Aktivitet> Aktiviteter { get; set; }


        // Dropdown

        /// <summary>
        /// AktivitetTyp för dropdown
        /// </summary>
        public List<SelectListItem> AktivitetTyper { get; set; }
    }
}
