﻿using LMS.Grupp4.Core.Entities;
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
                    lsItems.Add(new SelectListItem { Text = aktivitetTyp.AktivitetTypNamn, Value = aktivitetTyp.AktivitetTypId.ToString() });
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
    }
}
