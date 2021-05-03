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
        public async Task<JsonResult> CheckModuleStartDate(DateTime startDatum, DateTime slutDatum, int kursId, int modulId)
        {
            bool bValid = false;

            // TODO Temp inför merging
           // return Json(true);

            var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(kursId);
            if (kurs != null)
            {
                DateTime dtStartDatumDate = startDatum.Date;
                DateTime dtSlutDatumDate = slutDatum.Date;

                if (dtStartDatumDate <= dtSlutDatumDate)
                {// Starttid och sluttid är ok. Kontrollera att tiderna är inom kursen
                    //if (modul.StartDatum >= kurs.StartDatum && modul.SlutDatum <= kurs.SlutDatum)

                       if (kurs.StartDatum.Date <= dtStartDatumDate && kurs.SlutDatum.Date >= dtSlutDatumDate)
                    {// Nya tiderna är inom modulens tidsram
                     // Kontrollera att nya tiderna inte överlappar med andra aktiviteter

                        bValid = true;
                        DateTime startDate;
                        DateTime endDate;

                        IEnumerable<Modul> moduler = await m_UnitOfWork.ModulRepository.GetKursModulerAsync(kursId);
                        foreach (var modul in moduler)
                        {
                            if (modul.Id != modulId)
                            {
                                startDate = modul.StartDatum.Date;
                                endDate = modul.SlutDatum.Date;

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
                                    return Json("Kursen har redan Moduler under valt tidsintervall");
                                }
                            }
                        }
                    }
                    else
                    {
                        return Json("Valda datum är utanför kursen tidsram");
                    }
                }
                else
                {
                    return Json("Startdatum måste vara innan slutdatum");
                }
            }

            return Json(bValid);
        }
        public async Task<JsonResult> CheckModuleSlutDate(DateTime StartDatum, DateTime SlutDatum, int KursId, int modulId)
        {
            bool bValid = false;

            // TODO Temp inför merging
           // return Json(true);

            var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(KursId);
            if (kurs != null)
            {
                DateTime dtStartDatumDate = StartDatum.Date;
                DateTime dtSlutDatumDate = SlutDatum.Date;

                if (dtStartDatumDate <= dtSlutDatumDate)
                {// Starttid och sluttid är ok. Kontrollera att tiderna är inom modulen

                    if (kurs.StartDatum.Date <= dtStartDatumDate && kurs.SlutDatum.Date >= dtSlutDatumDate)
                    {// Nya tiderna är inom modulens tidsram
                     // Kontrollera att nya tiderna inte överlappar med andra aktiviteter

                        bValid = true;
                        DateTime startDate;
                        DateTime endDate;

                        IEnumerable<Modul> moduler = await m_UnitOfWork.ModulRepository.GetKursModulerAsync(KursId);
                        foreach (var modul in moduler)
                        {
                            if (modul.Id != modulId)
                            {
                                startDate = modul.StartDatum.Date;
                                endDate = modul.SlutDatum.Date;

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
                                    return Json("kursen har redan moduler under valt tidsintervall");
                                }
                            }
                        }
                    }
                    else
                    {
                        return Json("Valda datum är utanför kursen tidsram");
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

