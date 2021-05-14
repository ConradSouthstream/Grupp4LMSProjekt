using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.ViewModels.KursLitteratur
{
    public class KursLitteraturEditLitteraturViewModel
    {
        /// <summary>
        /// Litteraturens primärnyckel
        /// </summary>
        public int LitteraturId { get; set; }

        /// <summary>
        /// Litteraturens titel
        /// </summary>
        [DisplayName("Titel")]
        [Required(ErrorMessage = "Ange titel")]
        public string Titel { get; set; }

        /// <summary>
        /// Litteraturens datum
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Utgivningsdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "Ange utgivningsdatum")]
        public DateTime UtgivningsDatum { get; set; }

        /// <summary>
        /// Beskrivning av litteraturen
        /// </summary>
        [DisplayName("Beskrivning")]
        [Required(ErrorMessage = "Skriv in en beskrivning")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [DisplayName("Hemsida")]
        public string Url { get; set; }

        /// <summary>
        /// ForeignKey till litteraturen Ämne i tabellen Amne
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange ämne")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Ni måste ange ämne")]
        public int AmneId { get; set; }

        /// <summary>
        /// ForeignKey till literaturens nivå i tabellen Niva
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange nivå")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Ni måste ange nivå")]
        public int NivaId { get; set; }

        /// <summary>
        /// Litteraturens ämens kategori
        /// </summary>
        public string Amne { get; set; }

        /// <summary>
        /// Litteraturens nivå
        /// </summary>
        public string Niva { get; set; }


        /// <summary>
        /// Amnen för dropdown
        /// </summary>
        public List<SelectListItem> Amnen { get; set; }

        /// <summary>
        /// Niva för dropdown
        /// </summary>
        public List<SelectListItem> Nivaer { get; set; }

        public bool NoLitteratur { get; set; } = true;


        /// <summary>
        /// Litteraturens författare
        /// </summary>
        public IEnumerable<KursLitteraturForfattareViewModel> Forfattare { get; set; }
    }
}
