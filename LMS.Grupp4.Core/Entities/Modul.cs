using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

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
        public int Id { get; set; }

        /// <summary>
        /// Modul namn
        /// </summary>
        [Required(ErrorMessage = "Modulen måste ha ett namn")]
        public string Namn { get; set; }

        /// <summary>
        /// Beskrivning av modul
        /// </summary>
        [Required(ErrorMessage = "Nu måste ha en beskrivning av Modulen")]
        public string Beskrivning{ get; set; }

        /// <summary>
        /// Tid när modul slutar
        /// </summary>
       // [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
       // [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]

        [Required(ErrorMessage = "Ni måste ange en sluttid för modulen")]
        [Remote("CheckModuleSlutDate", "Validation",AdditionalFields ="KursId")]
        public DateTime SlutDatum { get; set; }

        /// <summary>
        /// Tid när modul starta
        /// </summary>
        //[DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ni måste ange en starttid för modulen")]
      //  [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]

        [Remote("CheckModuleStartDate","Validation",AdditionalFields ="KursId")]
        public DateTime StartDatum { get; set; }
        public int KursId { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> GetKursNamn { get; set; }

        //Navigation property
        public Kurs Kurs { get; set; }
        public ICollection<Aktivitet> Aktiviteter { get; set; }
        public ICollection<Dokument> Dokument { get; set; }


    }
}
