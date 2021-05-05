using LMS.Grupp4.Core.Entities;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IElevRepository
    {
        /// <summary>
        /// Metoden returnerar sökt Anvandare
        /// </summary>
        /// <param name="strElevId">Elevens id</param>
        /// <returns>Information om användaren eller null</returns>
        Task<Anvandare> GetAnvandareAsync(string strElevId);

        /// <summary>
        /// Async metod som returnerar Kurs som en elev deltar i
        /// </summary>
        /// <param name="strElevId">Elevens id</param>
        /// <returns>En elevs kurs</returns>
        Task<Kurs> GetKursAsync(string strElevId);

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();
    }
}
