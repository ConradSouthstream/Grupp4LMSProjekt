using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.TagHelpers
{
     [HtmlTargetElement("FilExtension")]
    public class DokumentExtension :TagHelper
    {
      
            //public bool Scale { get; set; } = false;
            //public DateTime StartDatum { get; set; }
            //public DateTime SlutDatum { get; set; }
            public string Namn { get; set; }
            public string ResultStatus { get; set; }

            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                output.TagName = "span";
                output.AddClass("Extension", HtmlEncoder.Default);
                var pdf = "/images/pdf-icon.png";
                var txt = "/images/txt-icon.png";
                var jpg = "/images/jpg-icon.png";
                var png = "/images/png-icon.png";
               // var doc = "/images/yellow.png";

                if (Namn.Contains(".txt"))
                {
                   
                        ResultStatus = $"<img src='{txt}' alt='Avslutad' title='Avslutad'  />";
                } if (Namn.Contains(".pdf"))
                {
                   
                        ResultStatus = $"<img src='{pdf}' alt='Avslutad' title='Avslutad'  />";
                } if (Namn.Contains(".jpg"))
                {
                   
                        ResultStatus = $"<img src='{jpg}' alt='Avslutad' title='Avslutad'  />";
                } if (Namn.Contains(".png"))
                {
                   
                        ResultStatus = $"<img src='{png}' alt='Avslutad' title='Avslutad' />";
                }
              

                output.Content.SetHtmlContent(ResultStatus);
            }

        }
    }
