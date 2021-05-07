using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace LMS.Grupp4.Web.TagHelpers
{

    [HtmlTargetElement("Status")]
    public class KursStatusIcon : TagHelper
    {
        public bool Scale { get; set; } = false;        
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }
        public string ResultStatus { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.AddClass("Status", HtmlEncoder.Default);
            var aktuell = "/images/green.png";
            var avslutade = "/images/red.png";
            var Kommande = "/images/yellow.png";

            if (SlutDatum < DateTime.Now)
            {
                if (Scale)
                    ResultStatus = $"<img src='{avslutade}' alt='Avslutad' title='Avslutad' style='width: 35%' />";
                else
                    ResultStatus = $"<img src='{avslutade}' alt='Avslutad' title='Avslutad' />";
            }
            if (StartDatum < DateTime.Now && DateTime.Now < SlutDatum)
            {
                if (Scale)
                    ResultStatus = $"<img src='{aktuell}' alt='Aktuell' title='Aktuell' style='width: 35%'/>";
                else
                    ResultStatus = $"<img src='{aktuell}' alt='Aktuell' title='Aktuell' />";
            }
            if (DateTime.Now < StartDatum)
            {
                if (Scale)
                    ResultStatus = $"<img src='{Kommande}' alt='Kommande' title='Kommande' style='width: 35%' />";
                else
                    ResultStatus = $"<img src='{Kommande}' alt='Kommande' title='Kommande' />";
            }

            output.Content.SetHtmlContent(ResultStatus);
        }

    }


}



