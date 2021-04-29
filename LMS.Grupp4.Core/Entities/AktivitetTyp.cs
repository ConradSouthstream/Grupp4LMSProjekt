using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// AktivitetTyp entitet
    /// </summary>
    public class AktivitetTyp
    {
        public int Id { get; set; }
        public string Namn { get; set; }

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
        //Navigation property
        public ICollection<Aktivitet> Aktiviteter { get; set; }
    }
}
