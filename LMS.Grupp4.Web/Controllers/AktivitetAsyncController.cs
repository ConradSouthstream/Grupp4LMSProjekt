using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class AktivitetAsyncController : Controller
    {
        private readonly IUnitOfWork m_UnitOfWork;

        public AktivitetAsyncController(IUnitOfWork uow)
        {
            m_UnitOfWork = uow;
        }

        /// <summary>
        /// Metoden kontrollera att startdatum och slutdatum är ok valda
        /// Ok innebär att startdatum är innan slutdatum.
        /// Aktiviteten startar och slutar inom modulen
        /// </summary>
        /// <param name="StartDatum">Aktivitetens StartDatum</param>
        /// <param name="SlutDatum">Aktivitetens SlutDatum</param>
        /// <param name="ModulId">Id för den modul som aktiviteten tillhör</param>
        /// <param name="AktivitetId">Aktivitetens id</param>
        /// <returns>Task med Json true om startdatum och slutdatum är ok annars returneras false</returns>
        public async Task<JsonResult> IfValidDatesEditStartDatum(DateTime StartDatum, DateTime SlutDatum, int ModulId, int AktivitetId)
        {
            bool bValid = false;

            // TODO Temp inför merging
            return Json(true);

            Modul modul = await m_UnitOfWork.ModulRepository.GetModulAsync(ModulId);
            if (modul != null)
            {
                DateTime dtStartDatumDate = StartDatum.Date;
                DateTime dtSlutDatumDate = SlutDatum.Date;

                if (dtStartDatumDate <= dtSlutDatumDate)
                {// Starttid och sluttid är ok. Kontrollera att tiderna är inom modulen

                    if (modul.StartDatum.Date <= dtStartDatumDate && modul.SlutDatum.Date >= dtSlutDatumDate)
                    {// Nya tiderna är inom modulens tidsram
                     // Kontrollera att nya tiderna inte överlappar med andra aktiviteter

                        bValid = true;
                        DateTime startDate;
                        DateTime endDate;

                        List<Aktivitet> lsAktiviteter = await m_UnitOfWork.AktivitetRepository.GetModulesAktivitetAsync(ModulId);
                        foreach (var aktivitet in lsAktiviteter)
                        {
                            if (aktivitet.Id != AktivitetId)
                            {
                                startDate = aktivitet.StartDatum.Date;
                                endDate = aktivitet.SlutDatum.Date;

                                if (startDate >= dtStartDatumDate && endDate >= dtStartDatumDate)
                                {// Aktivitetens StartDatum finns inom en tidigare aktivitet
                                    bValid = false;
                                }
                                else if (startDate >= dtSlutDatumDate && endDate >= dtSlutDatumDate)
                                {// Aktivitetens SlutDatum finns inom en tidigare aktivitet
                                    bValid = false;
                                }

                                if (bValid == false)
                                {
                                    return Json("Modulen har redan aktivitet under valt tidsintervall");
                                }
                            }
                        }
                    }
                    else
                    {
                        return Json("Valda datum är utanför modulens tidsram");
                    }
                }
                else
                {
                    return Json("Startdatum måste vara innan slutdatum");
                }
            }

            return Json(bValid);
        }


        /// <summary>
        /// Metoden kontrollera att startdatum och slutdatum är ok valda
        /// Ok innebär att startdatum är innan slutdatum.
        /// Aktiviteten startar och slutar inom modulen
        /// </summary>
        /// <param name="StartDatum">Aktivitetens StartDatum</param>
        /// <param name="SlutDatum">Aktivitetens SlutDatum</param>
        /// <param name="ModulId">Id för den modul som aktiviteten tillhör</param>
        /// <param name="AktivitetId">Aktivitetens id</param>
        /// <returns>Task med Json true om startdatum och slutdatum är ok annars returneras false</returns>
        public async Task<JsonResult> IfValidDatesEditSlutDatum(DateTime StartDatum, DateTime SlutDatum, int ModulId, int AktivitetId)
        {
            bool bValid = false;

            // TODO Temp inför merging
            return Json(true);

            Modul modul = await m_UnitOfWork.ModulRepository.GetModulAsync(ModulId);
            if (modul != null)
            {
                DateTime dtStartDatumDate = StartDatum.Date;
                DateTime dtSlutDatumDate = SlutDatum.Date;

                if (dtStartDatumDate <= dtSlutDatumDate)
                {// Starttid och sluttid är ok. Kontrollera att tiderna är inom modulen

                    if (modul.StartDatum.Date <= dtStartDatumDate && modul.SlutDatum.Date >= dtSlutDatumDate)
                    {// Nya tiderna är inom modulens tidsram
                     // Kontrollera att nya tiderna inte överlappar med andra aktiviteter

                        bValid = true;
                        DateTime startDate;
                        DateTime endDate;

                        List<Aktivitet> lsAktiviteter = await m_UnitOfWork.AktivitetRepository.GetModulesAktivitetAsync(ModulId);
                        foreach (var aktivitet in lsAktiviteter)
                        {
                            if (aktivitet.Id != AktivitetId)
                            {
                                startDate = aktivitet.StartDatum.Date;
                                endDate = aktivitet.SlutDatum.Date;

                                if (startDate >= dtStartDatumDate && endDate >= dtStartDatumDate)
                                {// Aktivitetens StartDatum finns inom en tidigare aktivitet
                                    bValid = false;
                                }
                                else if (startDate >= dtSlutDatumDate && endDate >= dtSlutDatumDate)
                                {// Aktivitetens SlutDatum finns inom en tidigare aktivitet
                                    bValid = false;
                                }

                                if (bValid == false)
                                {
                                    return Json("Modulen har redan aktivitet under valt tidsintervall");
                                }
                            }
                        }
                    }
                    else
                    {
                        return Json("Valda datum är utanför modulens tidsram");
                    }
                }
                else
                {
                    return Json("Startdatum måste vara innan slutdatum");
                }
            }

            return Json(bValid);
        }
    }
}
