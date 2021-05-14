using AutoMapper;
using Grupp4Lms.Core.Dto;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.KursLitteratur;
using LMS.Grupp4.Web.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
                HttpResponseMessage responseAmne = await m_HttpClient.GetAsync($"api/Kurslitteratur/GetAmnen");
                responseAmne.EnsureSuccessStatusCode();

                if (responseAmne.IsSuccessStatusCode)
                {
                    var strResultAmne = await responseAmne.Content.ReadAsStringAsync();

                    if (strResultAmne.StartsWith("[{") && strResultAmne.EndsWith("}]"))
                    {// Vi har en array av json objekt

                        lsAmnen = JsonConvert.DeserializeObject<List<AmneDto>>(strResultAmne, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                    }
                    else if (strResultAmne.StartsWith("{") && strResultAmne.EndsWith("}"))
                    {// Vi har bara ett json objekt

                        amneDto = JsonConvert.DeserializeObject<AmneDto>(strResultAmne, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
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
        /// Async action som söker efter litteratur
        /// om anropet till action kommer från view skall actionFrom = 1. Då utförs en sökning efter litteratur
        /// </summary>
        /// <param name="titel">Söker på litteratus titel och ser om det innehåller titel texten. Är det null eller tom sträng söks det ej på denna</param>
        /// <param name="forfattare">Söker på författares förnamn och efternamn och ser om det innehåller forfattare texten. Är det null eller tom sträng söks det ej på denna</param>
        /// <param name="AmneId">Söker på ämne. Om amne inte är större än 0 söks det inte på ämne</param>
        /// <param name="actionFrom">actionFrom=1 om anropet till action kommer från view. Då görs det en sökning mot web API. Annars kommer vi hoppa över sökningen</param>
        /// <returns>View</returns>
        public async Task<ActionResult<LitteraturListViewModel>> SearchLitteratur(string titel, string forfattare, int AmneId = -1, int actionFrom = -1)
        {
            LitteraturListViewModel model = new LitteraturListViewModel();

            try
            {                
                // Hämta ämnen från webapi. Visas i dropdown
                List<AmneDto> lsAmnen = await GetAmneAsync();                

                if (actionFrom == 1)
                {
                    // Sök efter kurslitteraturen
                    model.Litteratur = await SearchForLitteraturAsync(titel, forfattare, AmneId);

                    // Se till att det vi har sökt på syns i view
                    model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen, AmneId.ToString());

                    if (!String.IsNullOrWhiteSpace(titel))
                        model.Titel = titel;

                    if (!String.IsNullOrWhiteSpace(forfattare))
                        model.Forfattare = forfattare;
                }
                else
                {
                    model.Amnen = KursLitteraturHelper.CreateAmneDropDown(lsAmnen);
                }
            }
            catch(Exception exc) 
            { }

            return View(model);
        }
    }
}
