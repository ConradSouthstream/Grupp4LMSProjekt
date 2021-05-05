using LMS.Grupp4.Core.Entities;
using System;

namespace LMS.Grupp4.Web.Utils
{
    public class KursHelper
    {
        /// <summary>
        /// Metoden beräknar vilken status kurs har
        /// Status kan vara Avslutad, Aktuell, Kommande
        /// </summary>
        /// <param name="modul">Kurs</param>
        /// <returns>enum Status</returns>
        public static Status CalculateStatus(Kurs kurs)
        {
            Status status = Status.Avslutad;

            if (kurs.SlutDatum < DateTime.Now && kurs.StartDatum < DateTime.Now)
            {
                status = Status.Avslutad;
            }
            else if (kurs.StartDatum <= DateTime.Now && kurs.SlutDatum.AddHours(23) >= DateTime.Now)
            {
                status = Status.Aktuell;
            }
            else if (kurs.StartDatum > DateTime.Now.AddDays(1) && kurs.SlutDatum > DateTime.Now)
            {
                status = Status.Kommande;
            }

            return status;
        }
    }
}
