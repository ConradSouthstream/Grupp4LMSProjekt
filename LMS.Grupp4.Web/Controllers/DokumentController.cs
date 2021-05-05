using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class DokumentController : Controller
    {
        private readonly IUnitOfWork uow;
        public DokumentController(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public IActionResult Index()
        {
            var listdokument = uow.DokumentRepository.GetAllDokument();
            return View(listdokument);
        }
        public IActionResult Upload()
        {
            return View();
        }

        public FileResult DownloadFile(string filename)
        {
            return uow.DokumentRepository.DownloadFile(filename);
        }


        [HttpPost]
        public async Task<IActionResult> Upload(Dokument upload)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await uow.DokumentRepository.Create(upload);
            TempData["msg"] = "File Uploaded successfully";

            await uow.CompleteAsync();


            return View();
        }

    }
}
