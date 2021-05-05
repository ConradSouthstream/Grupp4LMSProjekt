using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Data;
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
        /// <param name="Id">Aktivitetens id. Om anropet kommer från create view kommer värdet inte vara satt dvs. får det defaulta värdet. Default värde är -1</param>
        /// <returns>Task med Json true om startdatum och slutdatum är ok annars returneras false</returns>
        public async Task<JsonResult> IfValidDatesEditStartDatum(DateTime StartDatum, DateTime SlutDatum, int ModulId, int Id = -1)
        {
            bool bValid = false;

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

                        foreach (Aktivitet ak in lsAktiviteter)
                        {
                            startDate = ak.StartDatum.Date;
                            endDate = ak.SlutDatum.Date;

                            // Om AktivitetId > 0 innebär det att anropar från Edit view. Är AktivitetId == -1 är det från Create view
                            if (Id > 0)
                            {// Vi gör validering från en Edit view och vi skall inte kolla samma aktivitet som vi vill ändra

                                if (ak.Id != Id)// Om tidigare aktivitet är samma som vi har i view kontrollerar jag inte
                                    bValid = Validate(dtStartDatumDate, dtSlutDatumDate, startDate, endDate);
                            }
                            else if (Id < 0)
                            {// Vi gör validering från en Create view
                                bValid = Validate(dtStartDatumDate, dtSlutDatumDate, startDate, endDate);
                            }

                            if (bValid == false)
                            {
                                return Json("Modulen har redan aktivitet under valt tidsintervall");
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
        /// <param name="Id">Aktivitetens id. Om anropet kommer från create view kommer värdet inte vara satt dvs. får det defaulta värdet. Default värde är -1</param>
        /// <returns>Task med Json true om startdatum och slutdatum är ok annars returneras false</returns>
        public async Task<JsonResult> IfValidDatesEditSlutDatum(DateTime StartDatum, DateTime SlutDatum, int ModulId, int Id = -1)
        {
            bool bValid = false;

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

                        foreach (Aktivitet ak in lsAktiviteter)
                        {
                            startDate = ak.StartDatum.Date;
                            endDate = ak.SlutDatum.Date;

                            // Om AktivitetId > 0 innebär det att anropar från Edit view. Är AktivitetId == -1 är det från Create view
                            if (Id > 0)
                            {// Vi gör validering från en Edit view och vi skall inte kolla samma aktivitet som vi vill ändra

                                if(ak.Id != Id)// Om tidigare aktivitet är samma som vi har i view kontrollerar jag inte
                                    bValid = Validate(dtStartDatumDate, dtSlutDatumDate, startDate, endDate);
                            }
                            else if(Id < 0)
                            {// Vi gör validering från en Create view
                                bValid = Validate(dtStartDatumDate, dtSlutDatumDate, startDate, endDate);
                            }

                            if (bValid == false)
                            {
                                return Json("Modulen har redan aktivitet under valt tidsintervall");
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
        /// Metoden validera att Datum för Start och Slut av en aktivitet inte kolliderar med en tidigare vald aktivitet
        /// </summary>
        /// <param name="dtStartDatumDate">Startdatum för Aktivitet i view</param>
        /// <param name="dtSlutDatumDate">Slutdatum för Aktivitet i view</param>
        /// <param name="startDate">Startdatum för tidigare aktivitet</param>
        /// <param name="endDate">Slutdatum för tidigare aktivitet</param>
        /// <returns>true om det inte finns några problem med valda datum. Annars returneras false</returns>
        private bool Validate(DateTime dtStartDatumDate, DateTime dtSlutDatumDate, DateTime startDate, DateTime endDate)
        {
            bool bValid = true;

            if (startDate <= dtStartDatumDate && endDate >= dtSlutDatumDate)
            {// Aktivitetens StartDatum och SlutDatum finns inom en tidigare aktivitet
                bValid = false;
                //Console.WriteLine($"Aktivitet {ak.Id}. StartDatum och SlutDatum finns inom en tidigare aktivitet");
            }
            else if (startDate <= dtStartDatumDate && endDate >= dtStartDatumDate)
            {// Aktivitetens StartDatum finns inom en tidigare aktivitet
                bValid = false;
                //Console.WriteLine($"Aktivitet {ak.Id}. StartDatum finns inom en tidigare aktivitet");
            }
            else if (startDate <= dtSlutDatumDate && endDate >= dtSlutDatumDate)
            {// Aktivitetens SlutDatum finns inom en tidigare aktivitet
                bValid = false;
                //Console.WriteLine($"Aktivitet {ak.Id}. SlutDatum finns inom en tidigare aktivitet");
            }
            else if (startDate >= dtStartDatumDate && endDate <= dtSlutDatumDate)
            {// Det finns en tidigare aktivitet inom den nya aktiviteten
                bValid = false;
                //Console.WriteLine($"Aktivitet {ak.Id}. Det finns en tidigare aktivitet inom aktiviteten");
            }

            return bValid;
        }
    }
}
