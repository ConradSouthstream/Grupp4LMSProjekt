using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.Entities
{
    public class AktivitetTyp
    {
        [Key]
        public int AktivitetTypId { get; set; }

        [Required]
        [StringLength(50)]
        public string AktivitetTypNamn { get; set; }
    }
}
