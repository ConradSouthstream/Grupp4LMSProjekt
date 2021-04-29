using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.Enum;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.Aktivitet;
using LMS.Grupp4.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class AktivitetController : BaseController
    {
        private readonly IUnitOfWork m_UnitOfWork;
        private readonly IMapper m_Mapper;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="uow">Unit of work. Används för att anropa olika Repository</param>
        /// <param name="mapper">Automapper</param>
        public AktivitetController(IUnitOfWork uow, IMapper mapper)
        {
            m_UnitOfWork = uow;
            m_Mapper = mapper;
        }

        // GET: AktivitetController
        public async Task<IActionResult> Index()
        {
            GetMessageFromTempData();

            List<Aktivitet> lsAktiviteter = m_UnitOfWork.AktivitetRepository.GetAktivitetAsync();
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
                Aktivitet aktivitet = m_UnitOfWork.AktivitetRepository.GetAktivitetAsync(id.Value);
                AktivitetDetailsViewModel viewModel = m_Mapper.Map<AktivitetDetailsViewModel>(aktivitet);
                return View(viewModel);
            }

            return View(null);
        }

        // GET: AktivitetController/Create/1
        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                AktivitetCreateViewModel viewModel = new AktivitetCreateViewModel();

                // Hämta information om Model
                Modul modul = m_UnitOfWork.ModulRepository.GetModulAsync(id.Value);
                if (modul != null)
                {
                    viewModel.ModulId = modul.ModulId;
                    viewModel.ModulNamn = modul.ModulNamn;                    
                    viewModel.ModulStartTid = modul.StartTid;
                    viewModel.ModulSlutTid = modul.SlutTid;
                }

                // Sätt upp startvärden för kalendrar
                DateTime dtNow = DateTime.Now;
                viewModel.StartTid = dtNow;
                viewModel.SlutTid = dtNow.AddDays(1);

                // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
                List<AktivitetTyp> lsAktivitetTyper = m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
                List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, viewModel.AktivitetTypId.ToString());
                viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

                return View(viewModel);
            }

            ViewBag.Message = "Det gick inte gå till sidan för att skapa aktivitet";
            ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

            return RedirectToAction(nameof(Index));
        }

        // POST: AktivitetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AktivitetCreateViewModel viewModel)
        {            
            if (ModelState.IsValid)
            {
                try
                {
                    Aktivitet aktivitet = m_Mapper.Map<Aktivitet>(viewModel);

                    // TODO implementera create mot Repository
                    // Post
                    // https://www.c-sharpcorner.com/article/http-get-put-post-and-delete-verbs-in-asp-net-web-api/
                    // Read = Get
                    // Update = Put
                    // Create = Post
                    // Delete = Delete

                    m_UnitOfWork.AktivitetRepository.PostAktivitetAsync(aktivitet);
                    if(m_UnitOfWork.AktivitetRepository.SaveAsync())
                    {// Vi har sparat en ny aktivitet. Redirect till listning

                        TempData["message"] = $"Har skapat aktivitet {viewModel.AktivitetNamn}";
                        TempData["typeOfMessage"] = TypeOfMessage.Info;

                        return RedirectToAction(nameof(Index));
                    }                    
                }
                catch (Exception) 
                { }
            }

            // Kommer vi hit har något gått fel
            ViewBag.Message = "Det gick inte skapa aktiviteten";
            ViewBag.TypeOfMessage = TypeOfMessage.Error;

            // Vi måste uppdatera viss data om modulen som inte view bindar till modellen
            Modul modul = m_UnitOfWork.ModulRepository.GetModulAsync(viewModel.ModulId);
            if (modul != null)
            {
                viewModel.ModulId = modul.ModulId;
                viewModel.ModulNamn = modul.ModulNamn;
                viewModel.ModulSlutTid = modul.SlutTid;
                viewModel.ModulStartTid = modul.StartTid;
            }

            // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
            List<AktivitetTyp> lsAktivitetTyper = m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
            List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, viewModel.AktivitetTypId.ToString());
            viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

            return View(viewModel);
        }


        // GET: AktivitetController/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id.HasValue)
            {
                Aktivitet aktivitet = m_UnitOfWork.AktivitetRepository.GetAktivitetAsync(id.Value);

                if (aktivitet != null)
                {
                    AktivitetEditViewModel viewModel = m_Mapper.Map<AktivitetEditViewModel>(aktivitet);

                    // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
                    List<AktivitetTyp> lsAktivitetTyper = m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
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
        public ActionResult Edit(int id, AktivitetEditViewModel viewModel)
        {
            if (id == viewModel.AktivitetId)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Aktivitet aktivitet = m_Mapper.Map<Aktivitet>(viewModel);

                        // Uppdatera aktivitet
                        m_UnitOfWork.AktivitetRepository.PutAktivitetAsync(aktivitet);

                        // Spara uppdateringen
                        if (m_UnitOfWork.AktivitetRepository.SaveAsync())
                        {
                            TempData["message"] = $"Har uppdaterat aktivitet {viewModel.AktivitetNamn}";
                            TempData["typeOfMessage"] = TypeOfMessage.Info;                           

                            return RedirectToAction(nameof(Index));
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
            Modul modul = m_UnitOfWork.ModulRepository.GetModulAsync(viewModel.ModulId);
            if(modul != null)
            {
                viewModel.ModulId = modul.ModulId;
                viewModel.ModulNamn = modul.ModulNamn;
                viewModel.ModulSlutTid = modul.SlutTid;
                viewModel.ModulStartTid = modul.StartTid;
            }

            // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
            List<AktivitetTyp> lsAktivitetTyper = m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
            List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, viewModel.AktivitetTypId.ToString());
            viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

            return View(viewModel);
        }

        // GET: AktivitetController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                Aktivitet aktivitet = m_UnitOfWork.AktivitetRepository.GetAktivitetAsync(id.Value);
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
        public ActionResult DeleteAktivitet(int? AktivitetId, string AktivitetNamn)
        {
            if (AktivitetId.HasValue)
            {
                try
                {
                    m_UnitOfWork.AktivitetRepository.DeleteAktivitetAsync(AktivitetId.Value);

                    if (m_UnitOfWork.AktivitetRepository.SaveAsync())
                    {// Aktiviteten är raderad

                        TempData["message"] = $"Raderade aktiviteten {AktivitetNamn}";
                        TempData["typeOfMessage"] = TypeOfMessage.Info;

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {// Det gick inte radera aktiviteten

                        Aktivitet aktivitet = m_UnitOfWork.AktivitetRepository.GetAktivitetAsync(AktivitetId.Value);
                        if (aktivitet != null)
                        {
                            AktivitetDeleteViewModel viewModel = m_Mapper.Map<AktivitetDeleteViewModel>(aktivitet);

                            ViewBag.Message = "Det gick inte radera aktiviteten";
                            ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

                            return View(nameof(Delete), viewModel);
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

            return RedirectToAction(nameof(Index));
        }
    }
}