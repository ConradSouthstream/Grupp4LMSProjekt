using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.Entities
{
    public class DokumentTyp
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public int DokumentId { get; set; }

        //Navigation property
        public ICollection<Dokument> Dokument { get; set; }
    }
}

