using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.Enum;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.Aktivitet;
using LMS.Grupp4.Data;
using LMS.Grupp4.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class AktivitetController : BaseController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="uow">Unit of work. Används för att anropa olika Repository</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="context">Databas context</param>
        public AktivitetController(IUnitOfWork uow, IMapper mapper, UserManager<Anvandare> userManager, ApplicationDbContext context) :
            base(uow, mapper, userManager)
        {
            _context = context;
        }

        // GET: AktivitetController
        public async Task<IActionResult> Index()
        {
            GetMessageFromTempData();

            List<Aktivitet> lsAktiviteter = await m_UnitOfWork.AktivitetRepository.GetAktiviteterAsync();
            List<AktivitetListViewModel> lsAktivitetListViewModel = new List<AktivitetListViewModel>();

            foreach (Aktivitet aktivitet in lsAktiviteter)
                lsAktivitetListViewModel.Add(m_Mapper.Map<AktivitetListViewModel>(aktivitet));

            return View(lsAktivitetListViewModel);
        }

        // GET: AktivitetController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                Aktivitet aktivitet = await m_UnitOfWork.AktivitetRepository.GetAktivitetIncludeKursAsync(id.Value);
                var dokument = await _context.Dokument.Include(d => d.Anvandare)
               .Where(d => d.AktivitetId == aktivitet.Id).ToListAsync();
                aktivitet.Dokument = dokument;
                AktivitetDetailsViewModel viewModel = m_Mapper.Map<AktivitetDetailsViewModel>(aktivitet);
                return View(viewModel);
            }

            return View(null);
        }

        // GET: AktivitetController/Create/1
        public async Task<ActionResult> Create(int? id)
        {
            if (id.HasValue)
            {
                AktivitetCreateViewModel viewModel = new AktivitetCreateViewModel();

                // Hämta information om Model
                Modul modul = await m_UnitOfWork.ModulRepository.GetModulWithAktiviteterAsync(id.Value);
                
                //Kolla om det finns aktiviteter på modulen, om det gör det ta det äldsta slutdatumet som startdatum.
                //Annars ta modulen startdatum.
                DateTime SenasteAktivitetDatum;
                if (modul.Aktiviteter.Any())
                { SenasteAktivitetDatum = modul.Aktiviteter.Max(m => m.SlutDatum).AddDays(1); }
                else { SenasteAktivitetDatum = modul.StartDatum; }

                if (modul != null)
                {
                    viewModel.ModulId = modul.Id;
                    viewModel.ModulNamn = modul.Namn;                    
                    viewModel.ModulStartDatum = modul.StartDatum;
                    viewModel.ModulSlutDatum = modul.SlutDatum;
                }

                viewModel.Aktiviteter = await m_UnitOfWork.AktivitetRepository.GetModulesAktivitetAsync(viewModel.ModulId);
                // Sätt upp startvärden för kalendrar
                DateTime dtNow = DateTime.Now;
                viewModel.StartDatum = SenasteAktivitetDatum;
                viewModel.SlutDatum = SenasteAktivitetDatum.AddDays(2);

                // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
                List<AktivitetTyp> lsAktivitetTyper = await m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
                List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, viewModel.AktivitetTypId.ToString());
                viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

                return View(viewModel);
            }

            ViewBag.Message = "Det gick inte gå till sidan för att skapa aktiviteten";
            ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

            return RedirectToAction(nameof(Index));
        }

        // POST: AktivitetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Namn, StartDatum, SlutDatum, Beskrivning, AktivitetTypId, ModulId")]AktivitetCreateViewModel viewModel)
        {            
            if (ModelState.IsValid)
            {
                try
                {
                    Aktivitet aktivitet = m_Mapper.Map<Aktivitet>(viewModel);
                    //aktivitet.Id = 0;

                    // Post
                    // https://www.c-sharpcorner.com/article/http-get-put-post-and-delete-verbs-in-asp-net-web-api/
                    // Read = Get
                    // Update = Put
                    // Create = Post
                    // Delete = Delete

                    await m_UnitOfWork.AktivitetRepository.PostAktivitetAsync(aktivitet);
                    if(await m_UnitOfWork.AktivitetRepository.SaveAsync())
                    {// Vi har sparat en ny aktivitet. Redirect till listning

                        TempData["message"] = $"Har skapat aktivitet {viewModel.Namn}";
                        TempData["typeOfMessage"] = TypeOfMessage.Info;
                                                
                        return RedirectToAction(nameof(Details), "Moduler", new { Id = viewModel.ModulId });
                    }                    
                }
                catch (Exception) 
                { }
            }

            // Kommer vi hit har något gått fel
            ViewBag.Message = "Det gick inte skapa aktiviteten";
            ViewBag.TypeOfMessage = TypeOfMessage.Error;

            // Vi måste uppdatera viss data om modulen som inte view bindar till modellen
            Modul modul = await m_UnitOfWork.ModulRepository.GetModulAsync(viewModel.ModulId);
            if (modul != null)
            {
                viewModel.ModulId = modul.Id;
                viewModel.ModulNamn = modul.Namn;
                viewModel.ModulSlutDatum = modul.SlutDatum;
                viewModel.ModulStartDatum = modul.StartDatum;
            }

            // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
            List<AktivitetTyp> lsAktivitetTyper = await m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
            List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, viewModel.AktivitetTypId.ToString());
            viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

            return View(viewModel);
        }


        // GET: AktivitetController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if(id.HasValue)
            {
                Aktivitet aktivitet = await m_UnitOfWork.AktivitetRepository.GetAktivitetAsync(id.Value);

                if (aktivitet != null)
                {
                    AktivitetEditViewModel viewModel = m_Mapper.Map<AktivitetEditViewModel>(aktivitet);

                    viewModel.Aktiviteter = await m_UnitOfWork.AktivitetRepository.GetModulesAktivitetAsync(viewModel.ModulId);

                    // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
                    List<AktivitetTyp> lsAktivitetTyper = await m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
                    List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, aktivitet.AktivitetTypId.ToString());
                    viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

                    return View(viewModel);
                }
            }

            return View(null);
        }

        // POST: AktivitetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AktivitetEditViewModel viewModel)
        {
            if (id == viewModel.Id)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Aktivitet aktivitet = m_Mapper.Map<Aktivitet>(viewModel);

                        // Uppdatera aktivitet
                        m_UnitOfWork.AktivitetRepository.PutAktivitet(aktivitet);

                        // Spara uppdateringen
                        if (await m_UnitOfWork.AktivitetRepository.SaveAsync())
                        {
                            TempData["message"] = $"Har uppdaterat aktivitet {viewModel.Namn}";
                            TempData["typeOfMessage"] = TypeOfMessage.Info;                           

                            //return RedirectToAction(nameof(Index));
                            return RedirectToAction(nameof(Details), "Moduler", new { Id = viewModel.ModulId });
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            // Kommer vi hit har något gått fel

            ViewBag.Message = "Det gick inte redigera aktiviteten";
            ViewBag.TypeOfMessage = TypeOfMessage.Error;

            // Vi måste uppdatera viss data om modulen som inte view bindar till modellen
            Modul modul = await m_UnitOfWork.ModulRepository.GetModulAsync(viewModel.ModulId);
            if(modul != null)
            {
                viewModel.ModulId = modul.Id;
                viewModel.ModulNamn = modul.Namn;
                viewModel.ModulSlutDatum = modul.SlutDatum;
                viewModel.ModulStartDatum = modul.StartDatum;
            }

            // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
            List<AktivitetTyp> lsAktivitetTyper = await m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
            List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, viewModel.AktivitetTypId.ToString());
            viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

            return View(viewModel);
        }

        // GET: AktivitetController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                Aktivitet aktivitet = await m_UnitOfWork.AktivitetRepository.GetAktivitetAsync(id.Value);
                if (aktivitet != null)
                {
                    AktivitetDeleteViewModel viewModel = m_Mapper.Map<AktivitetDeleteViewModel>(aktivitet);
                    return View(viewModel);
                }
            }

            return View(null);
        }

        // POST: AktivitetController/DeleteAktivitet/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAktivitet(int? Id, string AktivitetNamn)
        {
            Aktivitet aktivitet = await m_UnitOfWork.AktivitetRepository.GetAktivitetAsync(Id.Value);
            AktivitetDeleteViewModel viewModel = m_Mapper.Map<AktivitetDeleteViewModel>(aktivitet);
            if (Id.HasValue)
            {
                try
                {
                    await m_UnitOfWork.AktivitetRepository.DeleteAktivitetAsync(Id.Value);

                    if (await m_UnitOfWork.AktivitetRepository.SaveAsync())
                    {// Aktiviteten är raderad

                        TempData["message"] = $"Har raderat aktiviteten: {AktivitetNamn}";
                        TempData["typeOfMessage"] = TypeOfMessage.Info;

                        return RedirectToAction(nameof(Details), "Moduler", new { Id = viewModel.ModulId });
                    }
                    else
                    {// Det gick inte radera aktiviteten

                        
                        if (aktivitet != null)
                        { 
                            ViewBag.Message = "Det gick inte radera aktiviteten";
                            ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

                            return RedirectToAction(nameof(Details), "Moduler", new { Id = viewModel.ModulId });
                        }
                    }
                }
                catch(Exception)
                {
                }
            }

            // Hamnar man här har något gått fel
            TempData["message"] = "Det gick inte radera aktiviteten";
            TempData["typeOfMessage"] = TypeOfMessage.Error;

            return RedirectToAction(nameof(Details), "Moduler", new { Id = viewModel.ModulId });
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
        public IActionResult Upload(int id)
        {
            var Dokument = new Dokument
            {
                GetDokumentTypNamn = GetDokumentTypNamn(),
                AktivitetId = id
            };
            return View(Dokument);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Dokument upload)
        {
            //if (!ModelState.IsValid)
            //{
            //    return NotFound();
            //}
            upload.Anvandare = await m_UserManager.GetUserAsync(User);

            // var dokument = m_Mapper.Map<Dokument>(upload);


            await m_UnitOfWork.DokumentRepository.Create(upload);

            await m_UnitOfWork.CompleteAsync();

            TempData["msg"] = "Filen har laddats upp";
            //return Redirect("/Elev/Details/"+ dokument.KursId);
            //return Redirect("/Elev/ModulDetails/" + upload.ModulId);
           // return Redirect("Aktivitet/Details/" + upload.AktivitetId);

             return View(upload);
        }
    }
}