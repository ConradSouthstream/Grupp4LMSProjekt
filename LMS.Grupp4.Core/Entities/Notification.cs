using System.Collections.Generic;

namespace LMS.Grupp4.Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<NotificationAnvandare> NotificationApplicationUsers { get; set; }
    }
}