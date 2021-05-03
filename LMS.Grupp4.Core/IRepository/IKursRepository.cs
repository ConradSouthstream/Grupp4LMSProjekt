using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IKursRepository
    {
        /// <summary>
        /// Asyn metod som returnerar alla Kurser
        /// </summary>
        /// <returns>Task med alla Kurser</returns>
        Task<List<Kurs>> GetAllKurserAsync();
        /// <summary>
        /// Async metod som returnerar sökt Kurs
        /// </summary>
        /// <param name="iKursId">Id för sökt Kurs</param>
        /// <returns>Task med sökt Kurs eller null</returns>
        Task<Kurs> GetKursAsync(int? iKursId);



        /// <summary>
        /// Async metod som returnerar true om kursen existerar med det givna id annars false
        /// </summary>
        /// <param name="ikursId">Id för den modul som vi vill ha Kursrna för</param>
        /// <returns>Task med List med Kurs som tillhör en sökt modul</returns>
        bool KursExists(int? ikursId);
        /// <summary>
        /// Metoden uppdaterar data om en Kurs
        /// </summary>
        /// <param name="Kurs">Referense till Kurs som skall uppdateras</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till Kurs är null</exception>
        void PutKurs(Kurs Kurs);

        /// <summary>
        /// Async metod som skapar en ny Kurs
        /// </summary>
        /// <param name="Kurs">Referense till Kurs som skall skapas</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till Kurs är null</exception>
        Task PostKursAsync(Kurs Kurs);

        /// <summary>
        /// Metoden raderar en Kurs
        /// </summary>
        /// <param name="kurs"> Kurs som skall raderas</param>
        void RemoveKursAsync(Kurs kurs);

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();
    }
}

