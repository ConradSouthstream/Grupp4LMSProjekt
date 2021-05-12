using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int Id { get; set; }

        /// <summary>
        /// Aktivitetens namn
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange namn på aktiviteten")]
        public string Namn { get; set; }

        /// <summary>
        /// Tid när aktiviteten starta
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en startdatum för aktiviteten")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDatum { get; set; }

        /// <summary>
        /// Tid när aktiviteten slutar
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en slutdatum för aktiviteten")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SlutDatum { get; set; }

        /// <summary>
        /// Beskrivning av aktiviteten
        /// </summary>
        [Required(ErrorMessage = "Ni måste ha en beskrivning av aktiviteten")]
        public string Beskrivning { get; set; }       

        /// <summary>
        /// Aktivitetens typ
        /// </summary>
        //[ForeignKey("AktivitetTyp")]
        public int AktivitetTypId { get; set; }

        //[ForeignKey("Modul")]
        public int ModulId { get; set; }


        //Navigation property
        public ICollection<Dokument> Dokument { get; set; }
        public Modul Modul { get; set; }
        public AktivitetTyp AktivitetTyp { get; set; }
    }
}