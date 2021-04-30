using LMS.Grupp4.Core.Entities;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IModulRepository
    {
        /// <summary>
        /// Async metod som returnerar sökt Modul eller null
        /// </summary>
        /// <param name="iModulId">ModulId för sökt Modul</param>
        /// <returns>Task med sökt Modul eller null</returns>
        Task<Modul> GetModulAsync(int iModulId);

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();
    }
}