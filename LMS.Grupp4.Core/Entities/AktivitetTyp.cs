using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// AktivitetTyp entitet
    /// </summary>
    public class AktivitetTyp
    {
        /// <summary>
        /// Primärnyckel id
        /// </summary>
        [Key]
        public int AktivitetTypId { get; set; }

        /// <summary>
        /// Namnet på aktivitettyp
        /// </summary>
        [Required(ErrorMessage ="Ni måste ange ett namn på aktivitetstypen")]
        [StringLength(50, ErrorMessage = "Max längd är 50 tecken")]
        public string AktivitetTypNamn { get; set; }
    }
}
