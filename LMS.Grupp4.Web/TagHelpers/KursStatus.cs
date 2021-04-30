using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.TagHelpers
{

    [HtmlTargetElement("Status")]
    public class KursStatus : TagHelper
    {
        public bool Aktuell { get; set; }
        public bool Avslutade { get; set; }
        public bool  Kommande { get; set; }
        public string  Result { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.AddClass("Status", HtmlEncoder.Default);

            var IsAktuell = Aktuell;

            var commons = "https://www.svgrepo.com/show/";
            var pågår = commons + "/imgages/pågår.png";
            var avslutade = commons + "/imgages/avslutade.png";
            //var Kommande = commons + "52322/traffic-cone.svg";

            if (IsAktuell==true)
            {
              Result  = $"<img src='{pågår}'/>";
            }
            
              Result = $" < img src ='{avslutade}' /> ";
            






            //var result = (IsAktuell == true) ? $"<img src='{pågår}'/>" : $"<img src='{avslutade}'/>";

            output.Content.SetHtmlContent(Result);
        }
    }
}
