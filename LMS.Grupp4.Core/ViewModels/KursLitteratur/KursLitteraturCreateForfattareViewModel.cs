using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.ViewModels.KursLitteratur
{
    public class KursLitteraturCreateForfattareViewModel
    {
        /// <summary>
        /// Författarens Förnamn
        /// </summary>
        [DisplayName("Förnamn")]
        [Required(ErrorMessage = "Ange författerens förnamn")]
        public string ForNamn { get; set; }

        /// <summary>
        /// Författarens Efternamn
        /// </summary>
        [DisplayName("Efternamn")]
        [Required(ErrorMessage = "Ange författerens efternamn")]
        public string EfterNamn { get; set; }


        /// <summary>
        /// Författarens födelsedatum
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("Födelsdatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "Ange författerens födelsedatum")]
        public DateTime FodelseDatum { get; set; }

        /// <summary>
        /// Id för den litteratur som författaren skall kopplas till vid edit, create och delete
        /// Sätts av klienten
        /// </summary>
        public int LitteraturId { get; set; }

        /// <summary>
        /// Litteraturens titel
        /// </summary>
        public string Titel { get; set; }

        /// <summary>
        /// Litteraturens utgivningsdatum
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayName("UtgivningsDatum")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime UtgivningsDatum { get; set; }

        /// <summary>
        /// Litteraturens ämens kategori
        /// </summary>
        public string Amne { get; set; }

        /// <summary>
        /// Litteraturens nivå
        /// </summary>
        public string Niva { get; set; }
    }
}
