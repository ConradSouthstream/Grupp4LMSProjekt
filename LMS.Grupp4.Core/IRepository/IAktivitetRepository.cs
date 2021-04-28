using System;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IAktivitetRepository
    {
        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();
    }
}
