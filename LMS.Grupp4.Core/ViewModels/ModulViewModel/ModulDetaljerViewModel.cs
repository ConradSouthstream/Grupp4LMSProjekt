using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Grupp4.Core.ViewModels.Modul
{
    public class ModulDetaljerViewModel
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Beskrivning { get; set; }
        public IEnumerable<SelectListItem> GetKursNamn { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDatum { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime SlutDatum { get; set; }
        public int KursId { get; set; }
        public string KursNamn { get; set; }
        public ICollection<LMS.Grupp4.Core.Entities.Aktivitet> Aktiviteter { get; set; }
        public ICollection<Dokument> Dokument { get; set; }



    }
}
