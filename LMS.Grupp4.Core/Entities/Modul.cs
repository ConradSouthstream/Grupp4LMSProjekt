using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Grupp4.Core.Entities
{
    public  class Modul
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }
        public string Beskrivning{ get; set; }
        public int? KursId { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> GetKursNamn { get; set; }

        //Navigation property
        public Kurs Kurs { get; set; }
        public ICollection<Aktivitet> Aktiviteter { get; set; }
        public ICollection<Dokument> Dokument { get; set; }


    }
}
