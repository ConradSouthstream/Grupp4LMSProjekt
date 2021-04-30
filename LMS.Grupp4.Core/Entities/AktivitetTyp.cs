using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <summary>
        /// Primärnyckel id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Namnet på aktivitettyp
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange ett namn på aktivitetstypen")]
        public string Namn { get; set; }

        //Navigation property
        public ICollection<Aktivitet> Aktiviteter { get; set; }
    }
}
