﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Modul entitet
    /// </summary>
    public class Modul
    {
        /// <summary>
        /// Primärnyckel id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Modul namn
        /// </summary>
        [Required(ErrorMessage = "Modulen måste ha ett namn")]
        public string Namn { get; set; }

        /// <summary>
        /// Beskrivning av modul
        /// </summary>
        [Required(ErrorMessage = "Nu måste ha en beskrivning av Modulen")]
        public string Beskrivning{ get; set; }

        /// <summary>
        /// Tid när modul slutar
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en sluttid för modulen")]
        public DateTime SlutDatum { get; set; }

        /// <summary>
        /// Tid när modul starta
        /// </summary>
        [Required(ErrorMessage = "Ni måste ange en starttid för modulen")]
        public DateTime StartDatum { get; set; }

        //Navigation property
        public Kurs Kurs { get; set; }
        public int KursId { get; set; }
        public ICollection<Aktivitet> Aktiviteter { get; set; }
        public ICollection<Dokument> Dokument { get; set; }
    }
}
