using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IAktivitetRepository
    {
        /// <summary>
        /// Asyn metod som returnerar alla aktivitetet
        /// </summary>
        /// <returns>Task med alla aktiviteter</returns>
        Task<List<Aktivitet>> GetAktiviteterAsync();

        /// <summary>
        /// Async metod som returnerar sökt Aktivitet
        /// </summary>
        /// <param name="iAktivitetId">Id för sökt aktivitet</param>
        /// <returns>Task med sökt aktivitet eller null</returns>
        Task<Aktivitet> GetAktivitetAsync(int iAktivitetId);

        /// <summary>
        /// Async metod som returnerar alla AktivitetTyper
        /// </summary>
        /// <returns>Task med List med alla AktivitetTyper</returns>
        Task<List<AktivitetTyp>> GetAktivitetTyperAsync();

        /// <summary>
        /// Async metod som returnerar Aktiviteter som tillhör en modul
        /// </summary>
        /// <param name="iModuleId">Id för den modul som vi vill ha aktivitetrna för</param>
        /// <returns>Task med List med Aktivitet som tillhör en sökt modul</returns>
        Task<List<Aktivitet>> GetModulesAktivitetAsync(int iModuleId);

        /// <summary>
        /// Metoden uppdaterar data om en Aktivitet
        /// </summary>
        /// <param name="aktivitet">Referense till Aktivitet som skall uppdateras</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till Aktivitet är null</exception>
        void PutAktivitet(Aktivitet aktivitet);

        /// <summary>
        /// Async metod som skapar en ny aktivitet
        /// </summary>
        /// <param name="aktivitet">Referense till Aktivitet som skall skapas</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till Aktivitet är null</exception>
        Task PostAktivitetAsync(Aktivitet aktivitet);

        /// <summary>
        /// Metoden raderar en Aktivitet
        /// </summary>
        /// <param name="iAktivitetId">Id för Aktivitet som skall raderas</param>
        Task<int> DeleteAktivitetAsync(int iAktivitetId);

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();
    }
}
