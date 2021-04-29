using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public DateTime UppladdningsDatum { get; set; }
        public int? KursId { get; set; }
        public int? ModulId { get; set; }
        public int? AktivitetId { get; set; }
        public int? DokumentTypId { get; set; }

        //Navigation property 
        public  Anvandare Anvandare { get; set; }
        public Kurs Kurs { get; set; }
        public DokumentTyp DokumentTyp { get; set; }
        public  Modul Modul { get; set; }
        public  Aktivitet  Aktivitet { get; set; }
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
