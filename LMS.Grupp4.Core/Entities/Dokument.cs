using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Grupp4.Core.Entities
{
    /// <summary>
    /// Dokument entitet
    /// </summary>
    public class Dokument
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public string Path { get; set; }
        public DateTime UppladdningsDatum { get; set; }
        public int? KursId { get; set; }
        public int? ModulId { get; set; }
        public int? AktivitetId { get; set; }
        public int DokumentTypId { get; set; }
        public int AnvandareId { get; set; }



        //Navigation property
        public Anvandare Anvandare { get; set; }
        public Kurs Kurs { get; set; }
        public DokumentTyp DokumentTyp { get; set; }
        public Modul Modul { get; set; }
        public Aktivitet Aktivitet { get; set; }


    }
}
