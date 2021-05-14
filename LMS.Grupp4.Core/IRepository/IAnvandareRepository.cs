using LMS.Grupp4.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IAnvandareRepository
    {
        /// <summary>
        /// Async metod som returnerar alla anvandare på en kurs
        /// </summary>
        /// <param name="iKursId">Kursens id</param>
        /// <returns>Lis med användare på en kurs</returns>
        Task<List<Anvandare>> GetAnvandarePaKursAsync(int iKursId);

        Task<List<Kurs>> GetKurserForAnvandareAsync(string anvandareId);
    }
}
