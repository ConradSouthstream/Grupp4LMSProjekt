using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.DokumentViewModel;
using LMS.Grupp4.Core.ViewModels.Elev;
using LMS.Grupp4.Data;
using LMS.Grupp4.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    [Authorize(Roles = "Elev")]
    public class ElevController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="uow">Unit of work. Används för att anropa olika Repository</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="userManager">UserManager</param>
        public ElevController(IUnitOfWork uow, IMapper mapper, UserManager<Anvandare> userManager, ApplicationDbContext context, IWebHostEnvironment env ) :
            base(uow, mapper, userManager)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Upload(int id)
        {
            var Dokument = new Dokument
            {
                GetDokumentTypNamn = GetDokumentTypNamn(),
                KursId = id
            };
            return View(Dokument);
        }
        private IEnumerable<SelectListItem> GetDokumentTypNamn()

        {
            var TypeName = _context.DokumentTyper;

            var GetTypNamn = new List<SelectListItem>();
            foreach (var type in TypeName)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Dokument upload)
        {
            if (upload.File==null)
            {
                TempData["msg"] = "Ni måste välja en fil";
                upload.GetDokumentTypNamn = GetDokumentTypNamn();

            return View(upload);
            }
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            
            upload.Anvandare = await m_UserManager.GetUserAsync(User);
            
           // var dokument = m_Mapper.Map<Dokument>(upload);


            await m_UnitOfWork.DokumentRepository.Create(upload);

            await m_UnitOfWork.CompleteAsync();
            TempData["msg"] = "Filen har laddats upp";

           // return Redirect("/Elev/Details/"+ upload.KursId);
            //return Redirect("/Kurser/Details/" + upload.KursId);

             return View(upload);
        }
        public async Task<IActionResult> Details(int? KursId)
        {
            if (KursId == null)
            {
                return NotFound();
            }
            var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(KursId);

            var dokument = await _context.Dokument.Include(d => d.Anvandare)
                .Where(d => d.KursId == kurs.Id).ToListAsync();
          
            kurs.Dokument = dokument;
            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
        }

        public async Task<ActionResult> GetAnvandarna(int? KursId)
        {
            AnvandarElevDetaljerViewModel viewModel = null;
            List<AnvandarDetaljerViewModel> lsAnvandare = null;

            if (KursId.HasValue)
            {
                viewModel = new AnvandarElevDetaljerViewModel();
                var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(KursId);
                viewModel.KursNamn = (kurs != null) ? kurs.Namn : String.Empty;

                var kursAnvandare = await m_UnitOfWork.AnvandareRepository.GetAnvandarePaKursAsync(KursId.Value);
                if (kursAnvandare?.Count > 0)
                {// Nu ska jag hämta information om vilken roll användarna har och ange om användaren är lärare eller inte   
                    // Roller som finns i filen Seed.cs är Elev och Lärare
                    IList<string> lsRoles = null;
                    foreach (Anvandare anv in kursAnvandare)
                    {
                        // Vi borde bara få en roll tillbaka, men är role lärare kommer jag att break foreach
                        lsRoles = await m_UserManager.GetRolesAsync(anv);
                        if (lsRoles?.Count > 0)
                        {
                            foreach (string strRole in lsRoles)
                            {
                                if (strRole == "Lärare")
                                {
                                    anv.IsLarare = true;
                                    break;
                                }
                                else
                                {
                                    anv.IsLarare = false;
                                }
                            }
                        }
                    }

                    lsAnvandare = new List<AnvandarDetaljerViewModel>(kursAnvandare.Count);

                    AnvandarDetaljerViewModel model = null;
                    // Nu har jag en lista med användar där vi även vet om användaren är lärare eller ej
                    foreach (Anvandare anv in kursAnvandare)
                    {
                        model = m_Mapper.Map<AnvandarDetaljerViewModel>(anv);
                        if (model != null)
                        {
                            // Hack
                            model.KursId = (kurs != null) ? kurs.Id : 0;
                            lsAnvandare.Add(model);
                        }
                    }

                    viewModel.Larare = lsAnvandare.Where(i => i.IsLarare == true).ToList();
                    viewModel.Elever = lsAnvandare.Where(i => i.IsLarare == false).ToList();
                }
            }

            return View(viewModel);
        }


        /// <summary>
        /// Action som hämtar information om en kurs som inloggad användare läser och returnerar en View
        /// </summary>
        /// <returns>View</returns>
        // GET: ElevController
        public async Task<ActionResult> Index()
        {
            var userId = m_UserManager.GetUserId(User);

            // Hämta all data från repository
            var anvandare = await m_UnitOfWork.ElevRepository.GetAnvandareAsync(userId);
            var kurs = await m_UnitOfWork.ElevRepository.GetKursAsync(userId);
            var moduler = await m_UnitOfWork.ModulRepository.GetKursModulerIncludeAktivitetAsync(kurs.Id);

            // Skapa viewmodel som skall skickas till view
            ElevDetailsViewModel viewModel = new ElevDetailsViewModel();

            // Har vi en användare. Mappa info och lägg till i viewmodel
            if (anvandare != null)
                viewModel = m_Mapper.Map<ElevDetailsViewModel>(anvandare);

            // Har vi en kurs. Mappa info och lägg till i viewmodel
            if (kurs != null)
            {
                kurs.KursStatus = KursHelper.CalculateStatus(kurs);
                viewModel.Kurs = m_Mapper.Map<KursElevDetailsViewModel>(kurs);
            }

            // Har vi moduler. Mappa info och lägg till i viewmodel
            if (moduler?.Count() > 0)
            {
                AktivitetElevDetailsViewModel aktivitetElevDetailsViewModel = null;
                List<AktivitetElevDetailsViewModel> lsAktivitetElevDetailsViewModel = null;
                ModulElevDetailsViewModel modulElevDetailsViewModel = null;

                List<ModulElevDetailsViewModel> lsViewModelModul = new List<ModulElevDetailsViewModel>(moduler.Count());
                foreach (Modul modul in moduler)
                {
                    modulElevDetailsViewModel = m_Mapper.Map<ModulElevDetailsViewModel>(modul);
                    modulElevDetailsViewModel.ModulStatus = ModulHelper.CalculateStatus(modul);
                    modulElevDetailsViewModel.KursNamn = (kurs != null) ? kurs.Namn : String.Empty;
                    modulElevDetailsViewModel.Aktiviteter = null;

                    // Har vi aktiviteter. Mappa info och lägg till i viewmodel
                    if (modul.Aktiviteter?.Count > 0)
                    {
                        lsAktivitetElevDetailsViewModel = new List<AktivitetElevDetailsViewModel>(modul.Aktiviteter.Count);
                        foreach (Aktivitet akt in modul.Aktiviteter)
                        {
                            aktivitetElevDetailsViewModel = m_Mapper.Map<AktivitetElevDetailsViewModel>(akt);
                            aktivitetElevDetailsViewModel.AktivitetStatus = AktivitetHelper.CalculateStatus(akt);
                            lsAktivitetElevDetailsViewModel.Add(aktivitetElevDetailsViewModel);
                        }

                        modulElevDetailsViewModel.Aktiviteter = lsAktivitetElevDetailsViewModel;
                    }

                    lsViewModelModul.Add(modulElevDetailsViewModel);
                }

                viewModel.Moduler = lsViewModelModul;
            }

            return View(viewModel);
        }
        public IActionResult UploadDokument(int id)
        {
            var Dokument = new Dokument
            {
                GetDokumentTypNamn = GetDokumentTypNamn(),
                ModulId = id
            };
            return View(Dokument);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDokument(Dokument upload)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            upload.Anvandare = await m_UserManager.GetUserAsync(User);

            // var dokument = m_Mapper.Map<Dokument>(upload);


            await m_UnitOfWork.DokumentRepository.Create(upload);

            await m_UnitOfWork.CompleteAsync();

            TempData["msg"] = "Filen har laddats upp";
            //return Redirect("/Elev/Details/"+ dokument.KursId);
            return Redirect("/Elev/ModulDetails/" + upload.ModulId);

            // return View(dokument);
        }
        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(_env.WebRootPath, "Uploads/") + filename;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", filename);
        }


        /// <summary>
        /// Action som hämtar information om en modul och returnerar en View
        /// </summary>
        /// <param name="ModulId">Modul id</param>
        /// <returns>View</returns>
        public async Task<IActionResult> ModulDetails(int? ModulId)
        {
            ModulElevDetailsViewModel viewModel = null;

            if (ModulId.HasValue)
            {
                // Hämta modul inklusive kursen från repository
                var modul = await m_UnitOfWork.ModulRepository.GetModulWithKursAsync(ModulId.Value);
                if (modul != null)
                {
                    // Mappa Modul till ViewModel
                    var dokument = await _context.Dokument.Include(d => d.Anvandare)
                        .Where(d => d.ModulId == modul.Id).ToListAsync();
                    modul.Dokument = dokument;
                    viewModel = m_Mapper.Map<ModulElevDetailsViewModel>(modul);
                    var kurs = modul.Kurs;

                    viewModel.ModulStatus = ModulHelper.CalculateStatus(modul);
                    viewModel.KursNamn = (kurs != null) ? kurs.Namn : String.Empty;

                    // Hämta modulens aktiviteter
                    var aktiviteter = await m_UnitOfWork.AktivitetRepository.GetModulesAktivitetAsync(ModulId.Value);
                    List<AktivitetElevDetailsViewModel> lsAktivitetElevDetailsViewModel = null;
                    AktivitetElevDetailsViewModel aktivitetElevDetailsViewModel = null;

                    // Har vi aktiviteter. Mappa info och lägg till i viewmodel
                    if (aktiviteter?.Count > 0)
                    {
                        lsAktivitetElevDetailsViewModel = new List<AktivitetElevDetailsViewModel>(aktiviteter.Count);

                        // Mappa aktivitet till ViewModel
                        foreach (Aktivitet akt in aktiviteter)
                        {
                            aktivitetElevDetailsViewModel = m_Mapper.Map<AktivitetElevDetailsViewModel>(akt);
                            aktivitetElevDetailsViewModel.AktivitetStatus = AktivitetHelper.CalculateStatus(akt);
                            aktivitetElevDetailsViewModel.KursNamn = viewModel.KursNamn;
                            lsAktivitetElevDetailsViewModel.Add(aktivitetElevDetailsViewModel);
                        }
                    }

                    viewModel.Aktiviteter = lsAktivitetElevDetailsViewModel;
                }
            }

            return View(viewModel);
        }


        /// <summary>
        /// Action som hämtar information om en aktivitet och returnerar en View
        /// </summary>
        /// <param name="AktivietetId">Aktivitetens id</param>
        /// <returns></returns>
        public async Task<IActionResult> AktivitetDetails(int? AktivietetId)
        {
            AktivitetElevDetailsViewModel viewModel = null;

            if (AktivietetId.HasValue)
            {
                Aktivitet aktivitet = await m_UnitOfWork.AktivitetRepository.GetAktivitetIncludeKursAsync(AktivietetId.Value);

                // Har vi aktiviteter. Mappa info och lägg till i viewmodel
                if (aktivitet != null)
                {
                    viewModel = m_Mapper.Map<AktivitetElevDetailsViewModel>(aktivitet);
                    viewModel.AktivitetStatus = AktivitetHelper.CalculateStatus(aktivitet);
                }
            }

            return View(viewModel);
        }
  
        //// GET: ElevController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ElevController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ElevController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ElevController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ElevController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ElevController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ElevController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
