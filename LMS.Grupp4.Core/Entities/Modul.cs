﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Modul entitet
    /// </summary>
    public class Modul
    {
        /// <summary>
        /// Primärnyckel id
        /// </summary>
        [Key]
        public int ModulId { get; set; }

        /// <summary>
        /// Modul namn
        /// </summary>
        [Required(ErrorMessage = "Modulen måste ha ett namn")]
        [StringLength(50, ErrorMessage = "Max längd är 50 tecken")]
        public string ModulNamn { get; set; }

        /// <summary>
        /// Tid när modul starta
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en starttid för modulen")]
        public DateTime StartTid { get; set; }

        /// <summary>
        /// Tid när modul slutar
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en sluttid för modulen")]
        public DateTime SlutTid { get; set; }

        /// <summary>
        /// Beskrivning av modul
        /// </summary>
        [Required(ErrorMessage = "Nu måste ha en beskrivning av Modulen")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        public string Beskrivning { get; set; }
    }
}
