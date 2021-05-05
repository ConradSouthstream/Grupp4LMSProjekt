using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.ViewModels.DokumentViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IDokumentRepository
    {
        Task<Dokument> Create(Dokument input);
        List<Dokument> GetAllDokument();
        FileResult DownloadFile(string filename);

        Task<bool> SaveAsync();


    }
}
