using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.TagHelpers
{

    [HtmlTargetElement("Status")]
    public class KursStatusIcon : TagHelper
    {
        //public List<string> StatusResult = new List<string>();
        public Status KursStatus { get; set; }
        public string ResultStatus { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.AddClass("Status", HtmlEncoder.Default);
            var aktuell = "/images/aktuell.png";
            var avslutade = "/images/avslutade.png";
            var Kommande = "/images/kommande.png";
            if (KursStatus == Status.Avslutad)
            {
                ResultStatus = $"<img src='{avslutade}'/>";
            }
            if (KursStatus == Status.Aktuell)
            {
                ResultStatus = $"<img src='{aktuell}'/>";
            }
            if (KursStatus == Status.Kommande)
            {
                ResultStatus = $"<img src='{Kommande}'/>";
            }
            output.Content.SetHtmlContent(ResultStatus);
        }

    }


}



