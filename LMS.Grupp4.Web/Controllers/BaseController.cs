using Microsoft.AspNetCore.Mvc;

namespace LMS.Grupp4.Web.Controllers
{
    /// <summary>
    /// Basklass för gemensamma Controller metoder
    /// </summary>
    public class BaseController : Controller
    {
        protected void GetMessageFromTempData()
        {
            var messageObject = TempData["message"];
            if (messageObject != null)
                ViewBag.Message = messageObject as string;

            var typeOfMessageObject = TempData["typeOfMessage"];
            if (typeOfMessageObject != null)
                ViewBag.TypeOfMessage = typeOfMessageObject as string;
            else
                ViewBag.TypeOfMessage = "info";
        }
    }
}
