using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Utils
{
    public class ModulHelper
    {
        /// <summary>
        /// Metoden skapar en dropwdown list för KursNamn
        /// </summary>
        /// <param name="kurser">Lista med kurser</param>
        /// <returns>Dropdown för KursNamn</returns>
        public static List<SelectListItem> CreateKursNamnTypDropDown(List<Kurs> kurser)
        {
            List<SelectListItem> lsItems = new List<SelectListItem>();
            lsItems.Add(new SelectListItem { Text = "Inget val", Value = "0" });
            if (kurser?.Count > 0)
            {
                foreach (Kurs kurs in kurser)
                    lsItems.Add(new SelectListItem { Text = kurs.Namn, Value = kurs.Id.ToString() });
            }

            return lsItems;
        }


        /// <summary>
        /// Metoden skapar en dropwdown list för AktivitetTyp
        /// </summary>
        /// <param name="kurser">Lista med AktivitetTyp</param>
        /// <param name="strSelectedKursNamn">Value för AktivitetTyp som skall vara vald i dropdown</param>
        /// <returns>Dropdown för AktivitetTyp</returns>
        public static List<SelectListItem> CreateKursNamnDropDown(List<Kurs> kurser, string strSelectedKursNamn)
        {
            List<SelectListItem> lsItems = CreateKursNamnTypDropDown(kurser);

            if (!String.IsNullOrWhiteSpace(strSelectedKursNamn))
            {
                var listItem = lsItems.Where(v => v.Value == strSelectedKursNamn).FirstOrDefault();
                if (listItem != null)
                    listItem.Selected = true;
            }
            return lsItems;
        }
    }
}
