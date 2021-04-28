using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Grupp4.Web.Controllers
{
    public class AktivitetAsyncController : Controller
    {
        private readonly IUnitOfWork m_UnitOfWork;

        public AktivitetAsyncController(IUnitOfWork uow)
        {
            m_UnitOfWork = uow;
        }

        // [Remote(action: "CompareFirstName", controller:"Garages",
        // AdditionalFields = nameof(LastName),
        // ErrorMessage ="First Name and Last Name cannot match")]

        /// <summary>
        /// Metoden kontrollera att starttid och slutiderna är ok valda
        /// Ok innebär att starttid är innan sluttid.
        /// Aktiviteten startar och slutar inom en modul
        /// 
        /// </summary>
        /// <param name="StartTid"></param>
        /// <param name="SlutTid"></param>
        /// <param name="ModulId"></param>
        /// <param name="AktivitetId"></param>
        /// <returns></returns>
        public JsonResult IfValidDatesEditStartTid(DateTime StartTid, DateTime SlutTid, int ModulId, int AktivitetId)
        {
            bool bValid = false;

            // TODO RADERA
            return Json(true);

            Modul modul = m_UnitOfWork.ModulRepository.GetModulAsync(ModulId);
            if(modul != null)
            {
                if(StartTid < SlutTid)
                {// Starttid och sluttid är ok. Kontrollera att tiderna är inom modulen

                    if(modul.StartTid <= StartTid && modul.SlutTid >= SlutTid)
                    {// Nya tiderna är inom modulens tidsram
                     // Kontrollera att nya tiderna inte överlappar med andra aktiviteter

                        // Hämta modulens aktiviteter
                        List<Aktivitet> lsModulAktiviteter = m_UnitOfWork.AktivitetRepository.GetModulesAktivitetAsync(ModulId);
                        foreach(Aktivitet aktivitet in lsModulAktiviteter)
                        {
                            // TODO SKRIV KLART
                            // aktivitet.StartTid
                            // aktivitet.SlutTid
                        }

                        // TODO Sätt på rätt plats
                        bValid = true;
                    }
                    else
                    {
                        bValid = false;
                        return Json("Valda tider är utanför modulens tidsram");
                    }
                }
                else
                {
                    bValid = false;
                    return Json("Starttiden måste vara innan sluttiden");
                }
            }

            return Json(bValid);
        }

        public JsonResult IfValidDatesEditSlutTid(DateTime StartTid, DateTime SlutTid, int ModulId, int AktivitetId)
        {
            bool bValid = true;

            // TODO RADERA
            return Json(true);

            Modul modul = m_UnitOfWork.ModulRepository.GetModulAsync(ModulId);
            if (modul != null)
            {
                if (StartTid < SlutTid)
                {
                    // TODO SKRIV KLART
                }
                else
                {
                    bValid = false;
                    return Json("");
                }
            }

            return Json("ERROR"); //Json(bValid);
        }
    }
}
