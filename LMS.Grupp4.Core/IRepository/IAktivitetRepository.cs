using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IAktivitetRepository
    {
        List<Aktivitet> GetAktivitetAsync();

        /// <summary>
        /// Metoden returnerar sökt Aktivitet
        /// </summary>
        /// <param name="iAktivitetId">Id för sökt aktivitet</param>
        /// <returns>Sökt aktivitet eller null</returns>
        Aktivitet GetAktivitetAsync(int iAktivitetId);

        /// <summary>
        /// Metoden returnerar alla AktivitetTyper
        /// </summary>
        /// <returns>List med alla AktivitetTyper</returns>
        List<AktivitetTyp> GetAktivitetTyperAsync();

        /// <summary>
        /// Metoden returnerar Aktivitet somk tillhör en modul
        /// </summary>
        /// <param name="iModuleId">Id för den modul som vi vill ha aktivitetrna för</param>
        /// <returns>List med Aktivitet som tillhör en sökt modul</returns>
        List<Aktivitet> GetModulesAktivitetAsync(int iModuleId);

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        //Task<bool> SaveAsync();
        bool SaveAsync(); // TODO Ändra till async

        /// <summary>
        /// Metoden uppdaterar data om en Aktivitet
        /// </summary>
        /// <param name="aktivitet">Referense till Aktivitet som skall uppdateras</param>
        void PutAktivitetAsync(Aktivitet aktivitet);
    }
}
