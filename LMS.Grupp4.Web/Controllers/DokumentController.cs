using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.DokumentViewModel;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class DokumentController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork uow;
        private readonly UserManager<Anvandare> _usermanager;
        private readonly ApplicationDbContext _db;

        public DokumentController(IUnitOfWork uow, UserManager<Anvandare> usermanager, ApplicationDbContext db,IMapper mapper)
        {
            this.uow = uow;
            _usermanager = usermanager;
            _db = db;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var listdokument = uow.DokumentRepository.GetAllDokument();
            return View(listdokument);
        }
        public IActionResult Upload()
        {
            var Dokument = new Dokument
            {
                GetDokumentTypNamn = GetDokumentTypNamn()
            };
            return View(Dokument);
        }

         
        public FileResult DownloadFile(string filename)
        {
            return uow.DokumentRepository.DownloadFile(filename);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload( DocumentInput upload)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            upload.Anvandare = await _usermanager.GetUserAsync(User);
            // upload.DokumentTypId = 1;
            // upload.DokumentTyp = GetDokumentTypNamn();
            var dokument = _mapper.Map<Dokument>(upload);

            await uow.DokumentRepository.Create(dokument);
            TempData["msg"] = "Filen har laddats upp";

            await uow.CompleteAsync();


            return View(dokument);
        }
        private  IEnumerable<SelectListItem> GetDokumentTypNamn()

        {
            //var TypeName =  uow.DokumentTypRepository.GetAllDokumentTyperAsync();
            var TypeName = _db.DokumentTyper;

            var GetTypNamn = new List<SelectListItem>();
            foreach (var type in  TypeName)
            {
                var newNamn = (new SelectListItem
                {
                    Text = type.Namn,
                    Value = type.Id.ToString(),
                });
                GetTypNamn.Add(newNamn);
            }
            return (GetTypNamn);
        }

    }
}
