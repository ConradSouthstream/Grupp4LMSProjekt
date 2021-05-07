using LMS.Grupp4.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IDokumentTypRepository
    {
        /// <summary>
        /// Asyn metod som returnerar alla Dokument
        /// </summary>
        /// <returns>Task med alla Dokuments typer</returns>
        Task<IEnumerable<DokumentTyp>> GetAllDokumentTyperAsync();
        /// <summary>
        /// Async metod som returnerar sökt DokumentTyp
        /// </summary>
        /// <param name="idokumentTypId">Id för sökt DokumentTyp</param>
        /// <returns>Task med sökt dokumentTyp eller null</returns>
        Task<DokumentTyp> GetDokumentTypAsync(int? idokumentTypId);



        /// <summary>
        /// Async metod som returnerar true om DokumentTyp existerar med det givna id annars false
        /// </summary>
        /// <param name="idokumentTypId">Id för sökt DokumentTyp</param>
        /// <returns>true om DokumentTyp existerar eller false</returns>
        bool DokumentTypExists(int? idokumentTypId);
        /// <summary>
        /// Metoden uppdaterar data om e dokumentTyp
        /// </summary>
        /// <param name="dokumentTyp">Referense till dokumentTyp som skall uppdateras</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till dokumentTyp är null</exception>
        void UpdateDokumentTyp(DokumentTyp dokumentTyp);

        /// <summary>
        /// Async metod som skapar en ny dokumentTyp
        /// </summary>
        /// <param name="dokumentTyp">Referense till dokumentTyp som skall skapas</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till dokumentTyp är null</exception>
        Task CreateDokumentTypAsync(DokumentTyp dokumentTyp);

        /// <summary>
        /// Metoden raderar en dokumentTyp
        /// </summary>
        /// <param name="dokumentTyp"> dokumentTyp som skall raderas</param>
        void RemoveDokumentTypAsync(DokumentTyp dokumentTyp);

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        Task<bool> SaveAsync();
    }
}
