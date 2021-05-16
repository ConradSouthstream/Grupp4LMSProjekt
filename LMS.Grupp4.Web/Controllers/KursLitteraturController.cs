using AutoMapper;
using Grupp4Lms.Core.Dto;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.Enum;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.KursLitteratur;
using LMS.Grupp4.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class KursLitteraturController : BaseController
    {
        private HttpClient m_HttpClient;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="uow">Unit of work. Används för att anropa olika Repository</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="userManager">UserManager</param>
        /// <param name="httpClientFactory">Factory för att skapa http klienter</param>
        public KursLitteraturController(IUnitOfWork uow, IMapper mapper, UserManager<Anvandare> userManager, IHttpClientFactory httpClientFactory) :
            base(uow, mapper, userManager)
        { 
            m_HttpClient = httpClientFactory.CreateClient("KursLitteraturWebApiHttpClient");
        }

        #region private metoder

        /// <summary>
        /// Async metod som hämtar alla nivåer från repository
        /// </summary>
        /// <returns>Lista med nivåer</returns>
        private async Task<List<NivaDto>> GetNivaerAsync()
        {
            List<NivaDto> lsNivaer = null;
            NivaDto nivaDto = null;

            try
            {
                HttpResponseMessage response = await m_HttpClient.GetAsync("api/Kurslitteratur/GetNivaer");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (strResult.StartsWith("[{") && strResult.EndsWith("}]"))
                    {// Vi har en array av json objekt

                        lsNivaer = JsonConvert.DeserializeObject<List<NivaDto>>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    }
                    else if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                    {// Vi har bara ett json objekt

                        nivaDto = JsonConvert.DeserializeObject<NivaDto>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        if (nivaDto != null)
                        {
                            lsNivaer = new List<NivaDto>(1);
                            lsNivaer.Add(nivaDto);
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            return lsNivaer;
        }


        /// <summary>
        /// Async metod som hämtar alla ämnen från repository
        /// </summary>
        /// <returns>Lista med ämnen</returns>
        private async Task<List<AmneDto>>GetAmneAsync()
        {
            List<AmneDto> lsAmnen = null;
            AmneDto amneDto = null;

            try
            {
                HttpResponseMessage response = await m_HttpClient.GetAsync("api/Kurslitteratur/GetAmnen");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (strResult.StartsWith("[{") && strResult.EndsWith("}]"))
                    {// Vi har en array av json objekt

                        lsAmnen = JsonConvert.DeserializeObject<List<AmneDto>>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    }
                    else if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                    {// Vi har bara ett json objekt

                        amneDto = JsonConvert.DeserializeObject<AmneDto>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        if (amneDto != null)
                        {
                            lsAmnen = new List<AmneDto>(1);
                            lsAmnen.Add(amneDto);
                        }
                    }                    
                }
            }
            catch(Exception exc) 
            { }

            return lsAmnen;
        }


        /// <summary>
        /// Async metod som gör en sökning efter litteratur mot repository
        /// Det går att söka på titel, författare och ämne
        /// </summary>
        /// <param name="titel">Söker på litteratus titel och ser om det innehåller titel texten. Är det null eller tom sträng söks det ej på denna</param>
        /// <param name="forfattare">Söker på författares förnamn och efternamn och ser om det innehåller forfattare texten. Är det null eller tom sträng söks det ej på denna</param>
        /// <param name="amne">Söker på ämne. Om amne inte är större än 0 söks det inte på ämne</param>
        /// <returns>Task med en lista på hittad litteratur inklusive författare</returns>
        private async Task<List<KursLitteraturListViewModel>> SearchForLitteraturAsync(string titel, string forfattare, int amne)
        {
            List<KursLitteraturListViewModel> lsLitteratur = null;
            KursLitteraturListViewModel litteratur = null;

            try
            {
                int? tmpAmne = amne;

                HttpResponseMessage response = await m_HttpClient.GetAsync($"api/Kurslitteratur/SearchLitteratur?titel={titel}&forfattare={forfattare}&amne={tmpAmne}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (!String.IsNullOrWhiteSpace(strResult))
                    {
                        if (strResult.StartsWith("[{") && strResult.EndsWith("}]"))
                        {// Vi har en array av json objekt

                            lsLitteratur = JsonConvert.DeserializeObject<List<KursLitteraturListViewModel>>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        }
                        else if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                        {// Vi har bara ett json objekt

                            litteratur = JsonConvert.DeserializeObject<KursLitteraturListViewModel>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                            if (litteratur != null)
                            {
                                lsLitteratur = new List<KursLitteraturListViewModel>(1);
                                lsLitteratur.Add(litteratur);
                            }
                        }
                    }
                }
            }
            catch(Exception exc) 
            { }

            return lsLitteratur;
        }


        /// <summary>
        /// Metoden sorterar en lista med litteratur enligt parametrarna SortOrder och SortBy
        /// Default sortering är Titel
        /// </summary>
        /// <param name="litteratur">Lista med litetratur som skall sorteras</param>
        /// <param name="SortOrder">Sorterings order. ascending eller descending</param>
        /// <param name="SortBy">Vilken parameter skall vi sortera med. Kan vara Titel, UtgivningsDatum, Amne eller Niva</param>
        /// <returns>Sorterad IEnumerable med litteratur </returns>
        private IEnumerable<KursLitteraturListViewModel> SortLitteratur(IEnumerable<KursLitteraturListViewModel> litteratur, string SortOrder, string SortBy)
        {
            // Nu skall jag sortera enligt användarens val
            switch (SortBy)
            {
                //case "Titel":
                //    break;
                case "UtgivningsDatum":
                    if (SortOrder == "ascending")
                        litteratur = litteratur.OrderBy(l => l.UtgivningsDatum);
                    else
                        litteratur = litteratur.OrderByDescending(l => l.UtgivningsDatum);

                    break;
                case "Amne":
                    if (SortOrder == "ascending")
                        litteratur = litteratur.OrderBy(l => l.Amne);
                    else
                        litteratur = litteratur.OrderByDescending(l => l.Amne);

                    break;
                case "Niva":
                    if (SortOrder == "ascending")
                        litteratur = litteratur.OrderBy(l => l.Niva);
                    else
                        litteratur = litteratur.OrderByDescending(l => l.Niva);

                    break;
                default:
                    if (SortOrder == "ascending")
                        litteratur = litteratur.OrderBy(l => l.Titel);
                    else
                        litteratur = litteratur.OrderByDescending(l => l.Titel);

                    break;
            }

            return litteratur;
        }

        #endregion // End of region private metoder

        #region SearchLitteratur

        /// <summary>
        /// Async action som söker efter litteratur
        /// om anropet till action kommer från view skall actionFrom = 1. Då utförs en sökning efter litteratur
        /// </summary>
        /// <param name="titel">Söker på litteratus titel och ser om det innehåller titel texten. Är det null eller tom sträng söks det ej på denna</param>
        /// <param name="forfattare">Söker på författares förnamn och efternamn och ser om det innehåller forfattare texten. Är det null eller tom sträng söks det ej på denna</param>
        /// <param name="AmneId">Söker på ämne. Om amne inte är större än 0 söks det inte på ämne</param>
        /// <param name="actionFrom">actionFrom=1 om anropet till action kommer från view. Då görs det en sökning mot web API. Annars kommer vi hoppa över sökningen</param>
        /// <param name="OrderAuthorByName">true om vi skall sortera författarena efter deras namn. Annars är den false. Default är den false</param>
        /// <param name="OrderAuthorByAge">true om vi skall sortera författarena efter deras ålder. Annars är den false. Default är den false</param>
        /// <param name="SortBy">Namn på det vi skall sortera på i litteratur. Kan var en tom sträng</param>
        /// <param name="OldSortBy">Namn på det vi tidigare har sorterat litteratur på. Kan var en tom sträng</param>
        /// <param name="SortOrder">Vilken ordning som data i view är sorterad. ascending eller descending. Deafult ascending</param>
        /// <returns>View</returns>
        public async Task<ActionResult<LitteraturListViewModel>> SearchLitteratur(string titel, string forfattare, int AmneId = -1, int actionFrom = -1, bool OrderAuthorByName = false, bool OrderAuthorByAge = false, string SortBy = "", string OldSortBy = "", string SortOrder = "")
        {
            LitteraturListViewModel model = new LitteraturListViewModel();

            GetMessageFromTempData();

            try
            {                
                // Hämta ämnen från webapi. Visas i dropdown
                List<AmneDto> lsAmnen = await GetAmneAsync();

                if (actionFrom == 1)
                {
                    // Sök efter kurslitteraturen
                    model.Litteratur = await SearchForLitteraturAsync(titel, forfattare, AmneId);

                    if (model.Litteratur != null && model.Litteratur.Count() > 0)
                    {
                        // Här sätter jag upp lite variabler för att få det att fungera på ett logiskt sätt
                        // Om vi inte har en sort order sätter vi den till descending.
                        // För default är att vi skall sortera ascending och det ändrar vi nedan
                        if (String.IsNullOrWhiteSpace(SortOrder))
                            SortOrder = "descending";

                        if (!String.IsNullOrWhiteSpace(OldSortBy) && !String.IsNullOrWhiteSpace(SortBy))
                        {
                            if (OldSortBy != SortBy)
                            {// Detta innebär att vi inte sorterar på samma variabel dvs.
                             // vi skall återgå till default sortering som är ascending
                                SortOrder = "descending";
                            }
                        }

                        // default sortering är på Titel. Är det inte satt från view sätter jag det här
                        if (String.IsNullOrWhiteSpace(SortBy))
                            SortBy = "Titel";


                        // SortOrder från view är den sortorder som visas i view.
                        // Så nästa skall vara den omvända
                        if (SortOrder == "descending")
                            SortOrder = "ascending";
                        else
                            SortOrder = "descending";


                        if (OrderAuthorByName == true || OrderAuthorByAge == true)
                        {// Vi skall sortera på författarens namn och/eller ålder

                            foreach (var litteratur in model.Litteratur)
                            {
                                if (litteratur.Forfattare != null && litteratur.Forfattare.Count() > 1)
                                {// Vi har författare som vi kan sortera på

                                    if (OrderAuthorByName)
                                    {// Vi skall sortera på namn
                                        litteratur.Forfattare = litteratur.Forfattare
                                            .OrderBy(n => n.Name);
                                    }

                                    if (OrderAuthorByAge)
                                    {// Vi skall sorterad på ålder
                                        if (OrderAuthorByName)
                                        {// Vi skall också sortera på namn. Så gör dessa tillsammans
                                            litteratur.Forfattare = litteratur.Forfattare
                                                .OrderBy(n => n.Name)
                                                .ThenBy(a => a.Age);
                                        }
                                        else
                                        {
                                            litteratur.Forfattare = litteratur.Forfattare
                                                .OrderBy(a => a.Age);
                                        }
                                    }
                                }
                            }
                        }

                        // Nu skall jag sortera enligt användarens val
                        model.Litteratur = SortLitteratur(model.Litteratur, SortOrder, SortBy);
                        // Spara denna sortering till old variabeln
                        OldSortBy = SortBy;
                    }

                    // Se till att det vi har sökt på syns i view
                    model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen, AmneId.ToString());

                    if (!String.IsNullOrWhiteSpace(titel))
                        model.Titel = titel;

                    if (!String.IsNullOrWhiteSpace(forfattare))
                        model.Forfattare = forfattare;

                    model.OrderAuthorByName = OrderAuthorByName;

                    model.OrderAuthorByAge = OrderAuthorByAge;
                }
                else
                {
                    model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen);
                }
            }
            catch(Exception exc) 
            { }


            model.SortBy = SortBy;
            model.SortOrder = SortOrder;
            model.OldSortBy = OldSortBy;

            if (User.IsInRole("Lärare"))
                return View("SearchLitteraturAdmin", model);

            // Elev view utan administrationsmöjligheter
            return View("SearchLitteratur", model);
        }

        #endregion  // End of region SearchLitteratur

        #region Create litteratur

        /// <summary>
        /// Async action som skapar ny litteratur
        /// </summary>
        /// <param name="model">Information om nya litteraturen från view</param>
        /// <returns>Om allt gick bra RedirectToAction("SearchLitteratur") annars  View("CreateLitteratur", model)</returns>
        [Authorize(Roles = "Lärare")]
        public async Task<IActionResult> CreateLitteratur(KursLitteraturEditLitteraturViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Spara nya litteraturen med ett anrop till Web api
                    HttpResponseMessage response = await m_HttpClient.PostAsJsonAsync("api/Kurslitteratur", model);
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["message"] = $"Har skapat litteratur med titel {model.Titel}";
                        TempData["typeOfMessage"] = TypeOfMessage.Info;

                        return RedirectToAction("SearchLitteratur");
                    }
                }
            }
            catch(Exception exc)
            { }


            // Hamnar jag här har något gått fel
            // Hämta ämnen från webapi. Visas i dropdown
            List<AmneDto> lsAmnen = await GetAmneAsync();
            model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen);

            // Hämta nivåer från webapi. Visas i dropdown
            List<NivaDto> lsNivaer = await GetNivaerAsync();
            model.Nivaer = KursLitteraturHelper.CreateNivaDropDown(lsNivaer);

            ViewBag.Message = $"Det gick inte skapa litteratur med titel {model.Titel}";
            ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

            return View("CreateLitteratur", model);
        }


        /// <summary>
        /// Async action som anropas när användaren vill skapa ny litteratur
        /// </summary>
        /// <returns>View</returns>
        [Authorize(Roles = "Lärare")]
        [HttpGet]
        public async Task<IActionResult> CreateLitteratur()
        {
            KursLitteraturEditLitteraturViewModel model = new KursLitteraturEditLitteraturViewModel();

            try
            {
                // Hämta ämnen från webapi. Visas i dropdown
                List<AmneDto> lsAmnen = await GetAmneAsync();
                model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen);

                // Hämta nivåer från webapi. Visas i dropdown
                List<NivaDto> lsNivaer = await GetNivaerAsync();
                model.Nivaer = KursLitteraturHelper.CreateNivaDropDown(lsNivaer);

                model.UtgivningsDatum = DateTime.Now;
            }
            catch (Exception exc)
            { }

            return View(model);
        }

        #endregion // End of region Create litteratur

        #region Edit litteratur

        /// <summary>
        /// Async action som uppdaterar information om litteratur
        /// </summary>
        /// <param name="model">Information om uppdaterad litteraturen från view</param>
        /// <returns>Om det gick bra return RedirectToAction("SearchLitteratur") annars returneras View(model)</returns>
        [Authorize(Roles = "Lärare")]
        public async Task<IActionResult> EditLitteratur(KursLitteraturEditLitteraturViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = await m_HttpClient.PutAsJsonAsync($"api/Kurslitteratur/PutLitteraturAsync/{model.LitteraturId}", model);
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["message"] = $"Har uppdaterat litteratur med titel {model.Titel}";
                        TempData["typeOfMessage"] = TypeOfMessage.Info;

                        return RedirectToAction("SearchLitteratur");
                    }
                }
            }
            catch(Exception exc) 
            { }


            // Hamnar vi här har något gått fel

            // Hämta ämnen från webapi. Visas i dropdown
            List<AmneDto> lsAmnen = await GetAmneAsync();
            model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen, model.AmneId.ToString());

            // Hämta nivåer från webapi. Visas i dropdown
            List<NivaDto> lsNivaer = await GetNivaerAsync();
            model.Nivaer = KursLitteraturHelper.CreateNivaDropDown(lsNivaer, model.NivaId.ToString());

            model.NoLitteratur = false;

            ViewBag.Message = $"Det gick inte uppdatera litteratur med titel {model.Titel}";
            ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

            return View(model);
        }


        /// <summary>
        /// Action som anropas när användaren vill redigera en litteratur
        /// </summary>
        /// <param name="id">Id för litteratur som vi söker</param>
        /// <returns>View</returns>
        [Authorize(Roles = "Lärare")]
        [HttpGet]
        public async Task<IActionResult> EditLitteratur(int id)
        {
            KursLitteraturEditLitteraturViewModel model = new KursLitteraturEditLitteraturViewModel();

            GetMessageFromTempData();

            try
            {
                // Hämta ämnen från webapi. Visas i dropdown
                List<AmneDto> lsAmnen = await GetAmneAsync();
                model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen);

                // Hämta nivåer från webapi. Visas i dropdown
                List<NivaDto> lsNivaer = await GetNivaerAsync();
                model.Nivaer = KursLitteraturHelper.CreateNivaDropDown(lsNivaer);

                // GetLitteraturInklusiveForfattare
                // Hämta sökt litteratur från web api
                HttpResponseMessage response = await m_HttpClient.GetAsync($"api/Kurslitteratur/GetLitteraturInklusiveForfattare/{id}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                    {// Vi har bara ett json objekt
                        model = JsonConvert.DeserializeObject<KursLitteraturEditLitteraturViewModel>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    }

                    // Om vi har fått tillbaka litteratur vill jag sätta förvalda dropdown till det som finns i litteratur objektet från web api
                    model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen, model.AmneId.ToString());
                    model.Nivaer = KursLitteraturHelper.CreateNivaDropDown(lsNivaer, model.NivaId.ToString());

                    model.NoLitteratur = false;
                }
            }
            catch (Exception exc) 
            { }


            return View(model);
        }

        #endregion  // End of region Edit litteratur

        #region Edit författare

        /// <summary>
        /// Action som anropas när användaren vill redigera informationen om en författare
        /// </summary>
        /// <param name="id">Id för författaren</param>
        /// <returns>View</returns>
        [Authorize(Roles = "Lärare")]
        [HttpGet]
        public async Task<IActionResult> EditForfattare(int ForfatterId, int LitteraturId)
        {
            KursLitteraturEditForfattareViewModel model = new KursLitteraturEditForfattareViewModel();

            GetMessageFromTempData();

            try
            {
                HttpResponseMessage response = await m_HttpClient.GetAsync($"api/Kurslitteratur/GetForfattare/{ForfatterId}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                    {// Vi har bara ett json objekt
                        model = JsonConvert.DeserializeObject<KursLitteraturEditForfattareViewModel>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    }

                    model.NoForfattare = false;
                    model.LitteraturId = LitteraturId;
                }
            }
            catch(Exception exc) 
            { }

            return View(model);
        }


        /// <summary>
        /// Action som anropas när användaren uppdaterar uppgifter om en författare
        /// </summary>
        /// <param name="model">Infomration om författaren från view</param>
        /// <returns>return RedirectToAction("EditLitteratur", new { id = model.LitteraturId }) med olika meddelanden</returns>
        [Authorize(Roles = "Lärare")]
        public async Task<IActionResult> EditForfattare(KursLitteraturEditForfattareViewModel model)
        {
            try
            {
                HttpResponseMessage response = await m_HttpClient.PutAsJsonAsync($"api/Kurslitteratur/PutForfattareAsync/{model.ForfatterId}", model);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = $"Har uppdaterat information om författaren {model.ForNamn} {model.EfterNamn}";
                    TempData["typeOfMessage"] = TypeOfMessage.Info;

                    return RedirectToAction("EditLitteratur", new { id = model.LitteraturId });
                }
            }
            catch(Exception exc) 
            { }

            // Hamnar jag här har något gått fel
            TempData["message"] = $"Det gick inte uppdatera författaren {model.ForNamn} {model.EfterNamn}";
            TempData["typeOfMessage"] = TypeOfMessage.Error;

            return RedirectToAction("EditLitteratur", new { id = model.LitteraturId });
        }

        #endregion  // End of region Edit författare

        #region Create Författare

        /// <summary>
        /// Anropas när användaren vill skapa en ny författare som kopplas till litteratur
        /// </summary>
        /// <param name="LitteraturId">Id för litteraturen</param>
        /// <returns></returns>
        [Authorize(Roles = "Lärare")]
        [HttpGet]
        public async Task<IActionResult> CreateForfattare(int LitteraturId)
        {
            KursLitteraturCreateForfattareViewModel model = new KursLitteraturCreateForfattareViewModel();

            GetMessageFromTempData();

            try
            {// /api/Kurslitteratur/PostForfattare
                HttpResponseMessage response = await m_HttpClient.GetAsync($"/api/Kurslitteratur/GetLitteratur/{LitteraturId}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (!String.IsNullOrWhiteSpace(strResult))
                    {
                        if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                        {// Vi har bara ett json objekt

                            model = JsonConvert.DeserializeObject<KursLitteraturCreateForfattareViewModel>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                        }
                    }

                    model.FodelseDatum = DateTime.Now;

                    return View(model);
                }
             }
            catch (Exception exc)
            { }


            // Hamnar jag här har något gått fel
            TempData["message"] = "Det gick inte skapa en författare";
            TempData["typeOfMessage"] = TypeOfMessage.Error;

            return RedirectToAction("EditLitteratur", new { id = LitteraturId });
        }


        /// <summary>
        /// Async action som skapar en ny författare
        /// </summary>
        /// <param name="model">Information om nya författern från view</param>
        /// <returns></returns>
        [Authorize(Roles = "Lärare")]
        public async Task<IActionResult> CreateForfattare(KursLitteraturCreateForfattareViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Spara nya författaren med ett anrop till Web api
                    HttpResponseMessage response = await m_HttpClient.PostAsJsonAsync("api/Kurslitteratur/PostForfattareKopplaTillLitteratur", model);
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["message"] = $"Har skapat författare {model.ForNamn} {model.EfterNamn}";
                        TempData["typeOfMessage"] = TypeOfMessage.Info;

                        return RedirectToAction("EditLitteratur", new { id = model.LitteraturId});
                    }
                }
            }
            catch (Exception exc)
            { }

            // Hamnar jag här har något gått fel
            ViewBag.Message = $"Det gick inte skapa författare {model.ForNamn} {model.EfterNamn}";
            ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

            return View("CreateForfattare", model);
        }


        #endregion // End of region Create Författare

        #region Radera författare
        /// <summary>
        /// Action som anropas när användaren vill radera en författare
        /// </summary>
        /// <param name="id">ForfatterId för författaren och LitteraturId för litteraturen som författern har skrivit</param>
        /// <returns>View</returns>
        [Authorize(Roles = "Lärare")]
        [HttpGet]
        public async Task<IActionResult> DeleteForfattare(int ForfatterId, int LitteraturId)
        {
            KursLitteraturEditForfattareViewModel model = new KursLitteraturEditForfattareViewModel();

            try
            {
                HttpResponseMessage response = await m_HttpClient.GetAsync($"api/Kurslitteratur/GetForfattare/{ForfatterId}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                    {// Vi har bara ett json objekt
                        model = JsonConvert.DeserializeObject<KursLitteraturEditForfattareViewModel>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    }

                    model.NoForfattare = false;
                    model.LitteraturId = LitteraturId;

                    //TempData["message"] = $"Har raderat författaren {model.ForNamn} {model.EfterNamn}";
                    //TempData["typeOfMessage"] = TypeOfMessage.Info;

                    //return RedirectToAction("SearchLitteratur");
                }
            }
            catch(Exception exc)
            { }

            //ViewBag.Message = $"Det gick inte skapa författare {model.ForNamn} {model.EfterNamn}";
            //ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

            return View(model);
        }

        /// <summary>
        /// Action som anropas när användaren skall radera en författare
        /// </summary>
        /// <param name="id">Model med information om författaren som skall raderas</param>
        /// <returns>View</returns>
        [Authorize(Roles = "Lärare")]
        public async Task<IActionResult> DeleteForfattare(KursLitteraturEditForfattareViewModel model)
        {
            try
            {
                // Radera författaren med ett anrop till Web api                
                HttpResponseMessage response = await m_HttpClient.DeleteAsync($"api/Kurslitteratur/DeleteForfattareAsync/{model.ForfatterId}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = $"Har raderat författare {model.ForNamn} {model.EfterNamn}";
                    TempData["typeOfMessage"] = TypeOfMessage.Info;

                    return RedirectToAction("SearchLitteratur");
                }
            }
            catch(Exception exc) 
            { }


            TempData["message"] = $"Det gick inte radera författare {model.ForNamn} {model.EfterNamn}";
            TempData["typeOfMessage"] = TypeOfMessage.Error;

            return RedirectToAction("EditLitteratur", new { id = model.LitteraturId });
        }

        #endregion  // End region Radera författare

        #region Radera litteratur
        /// <summary>
        /// Action som anropas när användaren skall radera litteraturen
        /// </summary>
        /// <param name="id">Model med information om litteraturen som skall raderas</param>
        /// <returns>View</returns>
        [Authorize(Roles = "Lärare")]
        public async Task<IActionResult> DeleteLitteratur(KursLitteraturEditLitteraturViewModel model)
        {
            try
            {
                // Radera litteraturen med ett anrop till Web api
                HttpResponseMessage response = await m_HttpClient.DeleteAsync($"api/Kurslitteratur/DeleteLitteraturAsync/{model.LitteraturId}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    TempData["message"] = $"Har raderat litteratur {model.Titel}";
                    TempData["typeOfMessage"] = TypeOfMessage.Info;

                    return RedirectToAction("SearchLitteratur");
                }
            }
            catch (Exception exc)
            { }


            // Hamnar man här har något gått fel
            TempData["message"] = $"Det gick inte raderat litteratur {model.Titel}";
            TempData["typeOfMessage"] = TypeOfMessage.Error;

            return RedirectToAction("SearchLitteratur");
        }



        [Authorize(Roles = "Lärare")]
        [HttpGet]
        public async Task<IActionResult> DeleteLitteratur(int LitteraturId)
        {
            KursLitteraturEditLitteraturViewModel model = new KursLitteraturEditLitteraturViewModel();

            try
            {
                // Hämta sökt litteratur från web api
                HttpResponseMessage response = await m_HttpClient.GetAsync($"api/Kurslitteratur/GetLitteratur/{LitteraturId}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var strResult = await response.Content.ReadAsStringAsync();

                    if (strResult.StartsWith("{") && strResult.EndsWith("}"))
                    {// Vi har bara ett json objekt
                        model = JsonConvert.DeserializeObject<KursLitteraturEditLitteraturViewModel>(strResult, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    }

                    model.NoLitteratur = false;
                }
            }
            catch (Exception exc)
            { }


            return View(model);
        }

        #endregion  // End of region Radera litteratur
    }
}
