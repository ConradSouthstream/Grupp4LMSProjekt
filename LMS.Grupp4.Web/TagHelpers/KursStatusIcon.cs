using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace LMS.Grupp4.Web.TagHelpers
{

    [HtmlTargetElement("Status")]
    public class KursStatusIcon : TagHelper
    {
        public Status KursStatus { get; set; }
        public string ResultStatus { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.AddClass("Status", HtmlEncoder.Default);
            var aktuell = "/images/green.png";
            var avslutade = "/images/red.png";
            var Kommande = "/images/yellow.png";
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



