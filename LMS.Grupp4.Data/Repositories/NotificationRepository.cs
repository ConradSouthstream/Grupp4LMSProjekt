using System.Collections.Generic;
using System.Linq;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Data.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LMS.Grupp4.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        public ApplicationDbContext _context { get; }
        public IWatchlistRepository _watchlistRepository { get; }

        private IHubContext<SignalServer> _hubContext;

        public NotificationRepository(ApplicationDbContext context, 
                                        IWatchlistRepository watchlistRepository,
                                        IHubContext<SignalServer> hubContext)
        {
            _context = context;
            _watchlistRepository = watchlistRepository;
            _hubContext = hubContext;
        }

        public void Create(Notification notification,int dokumentId)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            //TODO: Assign notification to users
            var watchlists = _watchlistRepository.GetWatchlistFromDokumentId(dokumentId);
            foreach (var watchlist in watchlists)
            {
                var userNotification = new NotificationAnvandare();
                userNotification.AnvandareId = watchlist.AnvandareId;
                userNotification.NotificationId = notification.Id;

                _context.AnvandareNotification.Add(userNotification);
                _context.SaveChanges();
            }

            _hubContext.Clients.All.SendAsync("displayNotification", "");
        }

        public List<NotificationAnvandare> GetUserNotifications(string userId)
        {
            return _context.AnvandareNotification.Where(u=>u.AnvandareId.Equals(userId) && !u.IsRead)
                                            .Include(n=>n.Notification)
                                            .ToList();
        }

        public void ReadNotification(int notificationId, string userId)
        {
            var notification = _context.AnvandareNotification
                                        .FirstOrDefault(n=>n.AnvandareId.Equals(userId) 
                                        && n.NotificationId==notificationId);
            notification.IsRead = true;
            _context.AnvandareNotification.Update(notification);
            _context.SaveChanges();
        }
    }
}