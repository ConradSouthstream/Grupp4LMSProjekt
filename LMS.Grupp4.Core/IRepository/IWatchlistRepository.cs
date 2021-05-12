using LMS.Grupp4.Core.Entities;
using System.Collections.Generic;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IWatchlistRepository
    {
        Watchlist GetWatchlist(int Id);
        void Create(Watchlist watchlist);
        List<Watchlist> GetUserWatchlist(string anvandareId);
        void Remove(Watchlist watchlist);
        bool CheckIfAlreadyExists(string anvandareId, int dokumentId);
        List<Watchlist> GetWatchlistFromDokumentId(int dokumentId);
    }
}