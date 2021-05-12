using LMS.Grupp4.Core.Entities;
using System.Collections.Generic;

namespace LMS.Grupp4.Core.IRepository
{
    public interface INotificationRepository
    {
        List<NotificationAnvandare> GetUserNotifications(string anvandareId);
        void Create(Notification notification, int dokumentId);
        void ReadNotification(int notificationId, string anvandareId);
    }
}