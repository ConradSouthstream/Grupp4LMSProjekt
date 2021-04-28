using System.ComponentModel.DataAnnotations;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Anvandar entitet
    /// </summary>
    public class Anvandare
    {
        /// <summary>
        /// Primärnyckel
        /// </summary>
        [Key]
        public int AnvandarId { get; set; }

        /// <summary>
        /// Användarens namn
        /// </summary>
        [Required(ErrorMessage = "Ni måste ge användaren ett namn")]
        [StringLength(50, ErrorMessage = "Max längd är 50 tecken")]
        public string AnvandarNamn { get; set; }

        /// <summary>
        /// Användarens epost
        /// </summary>
        [Required(ErrorMessage = "Ni måste skriva en epost")]
        [StringLength(50, ErrorMessage = "Max längd är 50 tecken")]
        public string Epost { get; set; }

        /// <summary>
        /// Användarens lösenord
        /// </summary>
        [Required(ErrorMessage = "Ni måste skriva ett lösenord")]
        [StringLength(255, ErrorMessage = "Max längd är 255 tecken")]
        public string PassWord { get; set; }
    }
}