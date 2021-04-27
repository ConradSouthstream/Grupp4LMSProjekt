using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Grupp4.Core.Entities
{
    public class Aktivitet
    {
        [Key]
        public int AktivitetId { get; set; }

        [Required]
        [StringLength(255)]
        public string AktivitetNamn { get; set; }

        [Required]
        public DateTime StartTid { get; set; }

        [Required]
        public DateTime SlutTid { get; set; }

        [Required]
        [StringLength(255)]
        public string Beskrivning { get; set; }


        [ForeignKey("AktivitetTyp")]
        public int AktivitetTypId { get; set; }
        public AktivitetTyp AktivitetTyp { get; set; }
    }
}
