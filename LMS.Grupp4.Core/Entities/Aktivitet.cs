using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Aktivitet entitet
    /// </summary>
    public class Aktivitet
    {
        /// <summary>
        /// Primärnyckel. Id
        /// </summary>
        [Key]
        public int AktivitetId { get; set; }

        /// <summary>
        /// Aktivitetens namn
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange namn på aktiviteten")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        public string AktivitetNamn { get; set; }

        /// <summary>
        /// Tid när aktiviteten starta
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en starttid för aktiviteten")]
        public DateTime StartTid { get; set; }

        /// <summary>
        /// Tid när aktiviteten slutar
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en sluttid för aktiviteten")]
        public DateTime SlutTid { get; set; }

        /// <summary>
        /// Beskrivning av aktiviteten
        /// </summary>
        [Required(ErrorMessage = "Ni måste ha en beskrivning av aktiviteten")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Aktivitetens typ
        /// </summary>
        [ForeignKey("AktivitetTyp")]
        public int AktivitetTypId { get; set; }
        public AktivitetTyp AktivitetTyp { get; set; }

        /// <summary>
        /// Modul som aktiviteten tillhör
        /// </summary>
        [ForeignKey("Modul")]
        public int ModulId { get; set; }
        public Modul Modul { get; set; }
    }
}
