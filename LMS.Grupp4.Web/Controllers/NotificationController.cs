using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LMS.Grupp4.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private INotificationRepository _notificationRepository;
        private UserManager<Anvandare> _userManager;
        public NotificationController(INotificationRepository notificationRepository,
                                        UserManager<Anvandare> userManager)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }

        public IActionResult GetNotification(){
            var userId = _userManager.GetUserId(HttpContext.User);
            var notification = _notificationRepository.GetUserNotifications(userId);
            return Ok(new{UserNotification = notification, Count = notification.Count});
        }

        public IActionResult ReadNotification(int notificationId){

            _notificationRepository.ReadNotification(notificationId,_userManager.GetUserId(HttpContext.User));

            return Ok();
        }
    }
}