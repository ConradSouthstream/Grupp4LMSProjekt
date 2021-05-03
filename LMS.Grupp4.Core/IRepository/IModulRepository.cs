using LMS.Grupp4.Core.Entities;
using System.Collections.Generic;
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
        Task<Modul> GetModulByKurs(int kurs,int iModulId);


        /// <summary>
        /// Async metod som returnerar Moduler som tillhör en kurs
        /// </summary>
        /// <param name="ikursId">Id för den kurs som vi vill ha modulerna för</param>
        /// <returns>Task med IEnumerable till Moduler som tillhör en sökt kurs</returns>
        Task<IEnumerable<Modul>> GetKursModulerAsync(int ikursId);
        /// <summary>
        /// Metoden uppdaterar data om en Modul
        /// </summary>
        /// <param name="modul">Referense till Modul som skall uppdateras</param>

        void UpdateModul(Modul modul);

        /// <summary>
        /// Async metod som skapar en ny modul
        /// </summary>
        /// <param name="modul">Referense till Modul som skall skapas</param>
        void AddModul(Modul modul);
        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();
    }
}