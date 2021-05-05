using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
   public interface IDokumentRepository
    {
        Task<Dokument> Create(DocumentInput input);

    }
}
