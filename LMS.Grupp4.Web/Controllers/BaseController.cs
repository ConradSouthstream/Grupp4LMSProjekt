using AutoMapper;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.Enum;
using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Grupp4.Web.Controllers
{
    /// <summary>
    /// Basklass för gemensamma Controller metoder
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly UserManager<Anvandare> m_UserManager;
        protected readonly IUnitOfWork m_UnitOfWork;
        protected readonly IMapper m_Mapper;

        public BaseController(IUnitOfWork uow, IMapper mapper, UserManager<Anvandare> userManager)
        {
            m_UserManager = userManager;
            m_UnitOfWork = uow;
            m_Mapper = mapper;
        }

        protected void GetMessageFromTempData()
        {
            var messageObject = TempData["message"];
            if (messageObject != null)
                ViewBag.Message = messageObject as string;

            var typeOfMessageObject = TempData["typeOfMessage"];
            if (typeOfMessageObject != null)
                ViewBag.TypeOfMessage = (int)typeOfMessageObject;
            else
                ViewBag.TypeOfMessage = (int)TypeOfMessage.Info;
        }
    }
}
