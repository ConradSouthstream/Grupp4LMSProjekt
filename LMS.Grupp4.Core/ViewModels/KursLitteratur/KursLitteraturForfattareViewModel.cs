﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Grupp4.Core.ViewModels.KursLitteratur
{
    public class KursLitteraturForfattareViewModel
    {
        /// <summary>
        /// Författarens id
        /// </summary>
        [DisplayName("Författarens id")]
        public int ForfatterId { get; set; }

        /// <summary>
        /// Författarens förnamn
        /// </summary>
        [DisplayName("Förnamn")]
        public string ForNamn { get; set; }

        /// <summary>
        /// Författarens efternamn
        /// </summary>
        [DisplayName("Efternamn")]
        public string EfterNamn { get; set; }

        /// <summary>
        /// Författarens ålder
        /// </summary>
        [DisplayName("Ålder")]
        public int Age { get; set; }

        /// <summary>
        /// Returnerar namn på författaren
        /// </summary>
        public string Name
        {
            get
            {
                return $"{ForNamn} {EfterNamn}";
            }
        }
    }
}
