using System.Collections.Generic;
using System.Linq;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.EntityFrameworkCore;


namespace LMS.Grupp4.Data.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private ApplicationDbContext _context;
        public WatchlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CheckIfAlreadyExists(string userId, int dokumentId)
        {
            return _context.Watchlists.Any(w=>w.AnvandareId.Equals(userId) && w.DokumentId == dokumentId);
        }

        public void Create(Watchlist watchlist)
        {
            _context.Watchlists.Add(watchlist);
            _context.SaveChanges();
        }

        public List<Watchlist> GetUserWatchlist(string userId)
        {
            return _context.Watchlists
                            .Include(w=>w.Dokument)
                            .Where(w=>w.Anvandare.Equals(userId))
                            .ToList();
        }

        public Watchlist GetWatchlist(int Id)
        {
            return _context.Watchlists.FirstOrDefault(w=>w.Id==Id);
        }

        public List<Watchlist> GetWatchlistFromDokumentId(int dokumentId)
        {
            return _context.Watchlists.Where(w=>w.DokumentId == dokumentId).ToList();
        }

        public void Remove(Watchlist watchlist)
        {
            _context.Watchlists.Remove(watchlist);
            _context.SaveChanges();
        }
    }
}