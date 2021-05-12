using Grupp4Lms.Core.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Grupp4.Web.Utils
{
    public class KursLitteraturHelper
    {
        /// <summary>
        /// Metoden skapar en dropwdown list för Amne
        /// </summary>
        /// <param name="lsAmnen">Lista med Amne</param>
        /// <returns>Dropdown för Amne</returns>
        public static List<SelectListItem> CreateAmneDropDown(List<AmneDto> lsAmnen)
        {
            List<SelectListItem> lsItems = new List<SelectListItem>();
            lsItems.Add(new SelectListItem { Text = "Inget val", Value = "0" });

            if (lsAmnen?.Count > 0)
            {
                foreach (AmneDto dto in lsAmnen)
                    lsItems.Add(new SelectListItem { Text = dto.Namn, Value = dto.AmneId.ToString() });
            }

            return lsItems;
        }


        /// <summary>
        /// Metoden skapar en dropwdown list för Amne
        /// </summary>
        /// <param name="lsAmnen">Lista med Amnen</param>
        /// <param name="strSelectedAmne">Value för Amne som skall vara vald i dropdown</param>
        /// <returns>Dropdown för Amnen</returns>
        public static List<SelectListItem> CreateAmneDropDown(List<AmneDto> lsAmnen, string strSelectedAmne)
        {
            List<SelectListItem> lsItems = CreateAmneDropDown(lsAmnen);

            if (!String.IsNullOrWhiteSpace(strSelectedAmne))
            {
                var listItem = lsItems.Where(v => v.Value == strSelectedAmne).FirstOrDefault();
                if (listItem != null)
                    listItem.Selected = true;
            }
            return lsItems;
        }


        /// <summary>
        /// Metoden skapar en dropwdown list för Nivå
        /// </summary>
        /// <param name="lsAmnen">Lista med Nivå</param>
        /// <returns>Dropdown för Nivå</returns>
        public static List<SelectListItem> CreateNivaDropDown(List<NivaDto> lsNivaer)
        {
            List<SelectListItem> lsItems = new List<SelectListItem>();
            lsItems.Add(new SelectListItem { Text = "Inget val", Value = "0" });

            if (lsNivaer?.Count > 0)
            {
                foreach (NivaDto dto in lsNivaer)
                    lsItems.Add(new SelectListItem { Text = dto.Namn, Value = dto.NivaId.ToString() });
            }

            return lsItems;
        }


        /// <summary>
        /// Metoden skapar en dropwdown list för Nivå
        /// </summary>
        /// <param name="lsAmnen">Lista med Nivå</param>
        /// <param name="strSelectedNiva">Value för Nivå som skall vara vald i dropdown</param>
        /// <returns>Dropdown för Nivå</returns>
        public static List<SelectListItem> CreateNivaDropDown(List<NivaDto> lsNivaer, string strSelectedNiva)
        {
            List<SelectListItem> lsItems = CreateNivaDropDown(lsNivaer);

            if (!String.IsNullOrWhiteSpace(strSelectedNiva))
            {
                var listItem = lsItems.Where(v => v.Value == strSelectedNiva).FirstOrDefault();
                if (listItem != null)
                    listItem.Selected = true;
            }
            return lsItems;
        }
    }
}
