namespace  LMS.Grupp4.Core.Entities
{
    public class NotificationAnvandare
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
        public string AnvandareId { get; set; }
        public Anvandare Anvandare { get; set; }
        public bool IsRead { get; set; } = false;
    }
}