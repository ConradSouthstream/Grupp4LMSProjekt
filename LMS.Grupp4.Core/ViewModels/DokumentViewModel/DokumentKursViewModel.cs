using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace LMS.Grupp4.Core.ViewModels.DokumentViewModel
{
    public class DokumentKursViewModel
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public int DokumentTypId { get; set; }
        public int? AnvandareId { get; set; }
        public int? KursId { get; set; }
        public Kurs Kurs { get; set; }
        public string Path { get; set; }

        public Anvandare Anvandare { get; set; }
        public IFormFile File { get; set; }
    }
}
