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
        Task<Dokument> UploadDokumentKurs(Dokument input, int? kursId, int? dokumentTypId);
        Task<Dokument> UploadDokumentModul(Dokument input, int kursId,int modulId);
        Task<Dokument> UploadDokument(Dokument input, int? dokumentTypId);


        List<Dokument> GetAllDokument();
        FileResult DownloadFile(string filename);

        Task<bool> SaveAsync();


    }
}
