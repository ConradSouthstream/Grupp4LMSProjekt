using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Grupp4.Web.Utils
{
    public class AktivitetHelper
    {
        /// <summary>
        /// Metoden skapar en dropwdown list för AktivitetTyp
        /// </summary>
        /// <param name="lsAktivitetTyper">Lista med AktivitetTyp</param>
        /// <returns>Dropdown för AktivitetTyp</returns>
        public static List<SelectListItem>CreateAktivitetTypDropDown(List<AktivitetTyp>lsAktivitetTyper)
        {
            List<SelectListItem> lsItems = new List<SelectListItem>();
            lsItems.Add(new SelectListItem { Text = "Inget val", Value = "0" });
            if(lsAktivitetTyper?.Count > 0)
            {
                foreach (AktivitetTyp aktivitetTyp in lsAktivitetTyper)
                    lsItems.Add(new SelectListItem { Text = aktivitetTyp.Namn, Value = aktivitetTyp.Id.ToString() });
            }

            return lsItems;
        }


        /// <summary>
        /// Metoden skapar en dropwdown list för AktivitetTyp
        /// </summary>
        /// <param name="lsAktivitetTyper">Lista med AktivitetTyp</param>
        /// <param name="strSelectedAktivitetTyp">Value för AktivitetTyp som skall vara vald i dropdown</param>
        /// <returns>Dropdown för AktivitetTyp</returns>
        public static List<SelectListItem> CreateAktivitetTypDropDown(List<AktivitetTyp> lsAktivitetTyper, string strSelectedAktivitetTyp)
        {
            List<SelectListItem> lsItems = CreateAktivitetTypDropDown(lsAktivitetTyper);

            if(!String.IsNullOrWhiteSpace(strSelectedAktivitetTyp))
            {
                var listItem = lsItems.Where(v => v.Value == strSelectedAktivitetTyp).FirstOrDefault();
                if (listItem != null)
                    listItem.Selected = true;
            }
            return lsItems;
        }

        /// <summary>
        /// Metoden beräknar vilken status Aktiviteten har
        /// Status kan vara Avslutad, Aktuell, Kommande
        /// </summary>
        /// <param name="modul">Aktivitet</param>
        /// <returns>enum Status</returns>
        public static Status CalculateStatus(Aktivitet aktivitet)
        {
            Status status = Status.Avslutad;

            if (aktivitet.SlutDatum < DateTime.Now && aktivitet.StartDatum < DateTime.Now)
            {
                status = Status.Avslutad;
            }
            else if (aktivitet.StartDatum <= DateTime.Now && aktivitet.SlutDatum.AddHours(23) >= DateTime.Now)
            {
                status = Status.Aktuell;
            }
            else if (aktivitet.StartDatum > DateTime.Now.AddDays(1) && aktivitet.SlutDatum > DateTime.Now)
            {
                status = Status.Kommande;
            }

            return status;
        }
    }
}
