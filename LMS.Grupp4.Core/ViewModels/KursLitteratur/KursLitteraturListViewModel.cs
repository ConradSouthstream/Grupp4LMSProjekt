using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.ViewModels.KursLitteratur
{    
    public class KursLitteraturListViewModel
    {
        /// <summary>
        /// Litteraturens id
        /// </summary>
        public int LitteraturId { get; set; }

        /// <summary>
        /// Litteraturens titel
        /// </summary>
        [DisplayName("Titel")]
        public string Titel { get; set; }

        /// <summary>
        /// Litteraturens utgivningsdatum
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Utgivningsdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime UtgivningsDatum { get; set; }

        /// <summary>
        /// Beskrivning av Litteraturen
        /// </summary>
        [DisplayName("Beskrivning")]
        public string Beskrivning { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [DisplayName("Hemsida")]
        public string Url { get; set; }

        /// <summary>
        /// Litteraturens ämens kategori
        /// </summary>
        [DisplayName("Ämne")]
        public string Amne { get; set; }

        /// <summary>
        /// Litteraturens nivå
        /// </summary>
        [DisplayName("Nivå")]
        public string Niva { get; set; }

        /// <summary>
        /// Litteraturens författare
        /// </summary>
        public IEnumerable<KursLitteraturForfattareViewModel> Forfattare { get; set; }
    }
}
