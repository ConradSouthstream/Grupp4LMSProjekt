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
        public async Task<JsonResult> CheckModuleStartDate(DateTime StartDatum, DateTime SlutDatum, int kursId)
        {
            bool bValid = false;
            var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(kursId);
            DateTime dtStartDatumDate = StartDatum.Date;
            DateTime dtSlutDatumDate = SlutDatum.Date;

            if (dtStartDatumDate >= kurs.StartDatum && dtSlutDatumDate <= kurs.SlutDatum)
            {// Starttid och sluttid är ok. Kontrollera att tiderna är inom kursen   
                bValid = true;
                IEnumerable<Modul> moduler = await m_UnitOfWork.ModulRepository.GetKursModulerAsync(kursId);
                if (moduler.Count() != 0)
                {
                    var LastModul = moduler.Last();
                    if (dtStartDatumDate > LastModul.SlutDatum)
                    {
                        bValid = true;
                        return Json(bValid);
                    }

                    return Json("Modul får inte överlappa andra modulers tidsramen");

                }
                return Json(bValid);
            }
            else
                return Json("Modul kan inte överlappa kursen tidsramen");
        }




        public async Task<JsonResult> CheckModuleSlutDate(DateTime StartDatum, DateTime SlutDatum, int kursId)
        {
            bool bValid = false;
            var kurs = await m_UnitOfWork.KursRepository.GetKursAsync(kursId);
            DateTime dtStartDatumDate = StartDatum.Date;
            DateTime dtSlutDatumDate = SlutDatum.Date;
            if (dtStartDatumDate >= kurs.StartDatum && dtSlutDatumDate <= kurs.SlutDatum)
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


    }

}

