using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Dokument entitet
    /// </summary>
    public class Dokument
    {
        /// <summary>
        /// Primärnyckeln
        /// </summary>
        [Key]
        public int DokumentId { get; set; }

        /// <summary>
        /// Dokumentets nammn
        /// </summary>
        [Required(ErrorMessage = "Dokumentet måste ha ett namn")]
        [StringLength(50, ErrorMessage = "Max längd är 50 tecken")]
        public string DokumentNamn { get; set; }

        /// <summary>
        /// Beskrivning av dokumentet
        /// </summary>
        [Required(ErrorMessage = "Ni måste ha en beskrivning av dokumentet")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Path och namn på dokumentets fil
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange sökväg och filnamn på dokumentet")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        public string FilePath { get; set; }

        /// <summary>
        /// Tid när dokumentet skapades/uppdaterades
        /// </summary>
        public DateTime TidsStampel { get; set; }

        /// <summary>
        /// Användare som har skapat dokumentet
        /// </summary>
        [ForeignKey("Anvandare")]
        public int AnvandarId { get; set; }
        public Anvandare Uppladdare { get; set; }
    }
}
