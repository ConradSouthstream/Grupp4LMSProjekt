using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class ValidationController : Controller
    {
        private readonly IUnitOfWork m_UnitOfWork;

        public ValidationController(IUnitOfWork uow)
        {
            m_UnitOfWork = uow;
        }
        /* Anther way to check if the RegistrationNum is unique */

        //[AcceptVerbs("GET", "POST")]
        //public IActionResult IsRegExists(string RegistrationNum, int Id)
        //{
        //    return Json(IsUnique(RegistrationNum, Id));
        //}

        //private bool IsUnique(string RegistrationNum, int Id)
        //{
        //    if (Id == 0) // its a new object
        //    {
        //        return !_context.ParkedVehicle.Any(x => x.RegistrationNum == RegistrationNum);
        //    }
        //    else 
        //    {
        //        return !_context.ParkedVehicle.Any(x => x.RegistrationNum == RegistrationNum && x.Id != Id);
        //    }
        //}
        public async Task<JsonResult> CheckModuleStartDate(DateTime StartDatum, DateTime SlutDatum, int kursId, int Id)
        {




            bool bValid = false;
            var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(kursId);
            DateTime dtStartDatumDate = StartDatum.Date;
            DateTime dtSlutDatumDate = SlutDatum.Date;
            if (Id == 0)
            {
                if (dtStartDatumDate >= kurs.StartDatum.Date && dtSlutDatumDate <= kurs.SlutDatum.Date)
                {// Starttid och sluttid är ok. Kontrollera att tiderna är inom kursen   
                    bValid = true;
                    IEnumerable<Modul> moduler = await m_UnitOfWork.ModulRepository.GetKursModulerAsync(kursId);
                    if (moduler?.Count() != 0)
                    {
                        var LastModul = moduler.Last();
                        if (dtStartDatumDate > LastModul.SlutDatum)
                        {
                            bValid = true;
                            return Json(bValid);
                        }

                        return Json("Modulen får inte överlappa andra modulers tidsram");

                    }
                    return Json(bValid);
                }
                else
                    return Json("Modul kan inte överlappa kursen tidsramen");
            }
            else
                if (dtStartDatumDate >= kurs.StartDatum.Date && dtSlutDatumDate <= kurs.SlutDatum.Date)
            {
                bValid = true;
                IEnumerable<Modul> moduler = await m_UnitOfWork.ModulRepository.GetKursModulerAsync(kursId);
                var listmodul = moduler.Where(m => m.Id != Id).ToList();
                if (listmodul.Exists(m => m.StartDatum == dtStartDatumDate || m.SlutDatum == dtSlutDatumDate))
                {
                      return Json("Modul får inte överlappa andra modulers tidsramen");

                }
              
                return Json(bValid);
            }
            return Json("Modul kan inte överlappa kursen tidsramen");
            

        }    
        public async Task<JsonResult> CheckModuleSlutDate(DateTime StartDatum, DateTime SlutDatum, int kursId)
        {
            bool bValid = false;
            var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(kursId);
            DateTime dtStartDatumDate = StartDatum.Date;
            DateTime dtSlutDatumDate = SlutDatum.Date;
            if (dtStartDatumDate >= kurs.StartDatum.Date && dtSlutDatumDate <= kurs.SlutDatum.Date)
            {// Starttid och sluttid är ok. Kontrollera att tiderna är inom kursen   
                if (dtStartDatumDate > dtSlutDatumDate)
                {
                    return Json("Slutdatum kan inte vara tidigare än startdatumet");
                }
                else
                    bValid = true;
                return Json(bValid);

            }
            return Json("Modul kan inte överlappa kursen tidsramen");


        }
        public async Task<JsonResult> IfValidDatesEditStartDatum(DateTime StartDatum, DateTime SlutDatum, int KursId, int Id = -1)
        {
            bool bValid = false;

            Kurs modul = await m_UnitOfWork.KursRepository.GetKursAsync(KursId);
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

                        var lsModuler = await m_UnitOfWork.ModulRepository.GetKursModulerAsync(KursId);

                        foreach (Modul mo in lsModuler)
                        {
                            startDate = mo.StartDatum.Date;
                            endDate = mo.SlutDatum.Date;

                            // Om modulId > 0 innebär det att anropar från Edit view. Är modulId == -1 är det från Create view
                            if (Id > 0)
                            {// Vi gör validering från en Edit view och vi skall inte kolla samma aktivitet som vi vill ändra

                                if (mo.Id != Id)// Om tidigare modul är samma som vi har i view kontrollerar jag inte
                                    bValid = Validate(dtStartDatumDate, dtSlutDatumDate, startDate, endDate);
                            }
                            else if (Id < 0)
                            {// Vi gör validering från en Create view
                                bValid = Validate(dtStartDatumDate, dtSlutDatumDate, startDate, endDate);
                            }

                            if (bValid == false)
                            {
                                return Json("Kursen har redan en modul under valt tidsintervall");
                            }
                        }
                    }
                    else
                    {
                        return Json("Valda datum är utanför kursens tidsram");
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
        public async Task<JsonResult> IfValidDatesEditSlutDatum(DateTime StartDatum, DateTime SlutDatum, int KursId, int Id = -1)
        {
            bool bValid = false;

            Kurs modul = await m_UnitOfWork.KursRepository.GetKursAsync(KursId);
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

                        var lsAktiviteter = await m_UnitOfWork.ModulRepository.GetKursModulerAsync(KursId);

                        foreach (Modul ak in lsAktiviteter)
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
                                return Json("Kursen har redan en modul under valt tidsintervall");
                            }
                        }
                    }
                    else
                    {
                        return Json("Valda datum är utanför kursens tidsram");
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

