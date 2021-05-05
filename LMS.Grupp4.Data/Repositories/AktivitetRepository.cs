using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Repositories
{
    /// <summary>
    /// AktivitetRepository med metoder för att hantera Aktivietet
    /// </summary>
    public class AktivitetRepository : IAktivitetRepository
    {
        /// <summary>
        /// Databas context
        /// </summary>
        private readonly ApplicationDbContext m_DbContext;


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Referense till context</param>
        public AktivitetRepository(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        /// <summary>
        /// Asyn metod som returnerar alla aktivitetet
        /// </summary>
        /// <returns>Task med alla aktiviteter</returns>
        public async Task<List<Aktivitet>> GetAktiviteterAsync()
        {
            return await m_DbContext.Aktiviteter
                .Include(m => m.Modul)
                .Include(a => a.AktivitetTyp)
                .ToListAsync();
        }

        /// <summary>
        /// Async metod som returnerar sökt Aktivitet
        /// </summary>
        /// <param name="iAktivitetId">Id för sökt aktivitet</param>
        /// <returns>Task med sökt aktivitet eller null</returns>
        public async Task<Aktivitet> GetAktivitetAsync(int iAktivitetId)
        {
            return await m_DbContext.Aktiviteter
                .Include(a => a.AktivitetTyp)
                .Include(m => m.Modul)                
                .Where(a => a.Id == iAktivitetId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async metod som returnerar sökt Aktivitet. Inkluderar Kurs som finns i Model
        /// </summary>
        /// <param name="iAktivitetId">Id för sökt aktivitet</param>
        /// <returns>Task med sökt aktivitet eller null</returns>
        public async Task<Aktivitet> GetAktivitetIncludeKursAsync(int iAktivitetId)
        {
            return await m_DbContext.Aktiviteter
                .Include(a => a.AktivitetTyp)
                .Include(m => m.Modul)
                .ThenInclude(k => k.Kurs)                
                .Where(a => a.Id == iAktivitetId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async metod som returnerar alla AktivitetTyper
        /// </summary>
        /// <returns>Task med List med alla AktivitetTyper</returns>
        public async Task<List<AktivitetTyp>> GetAktivitetTyperAsync()
        {
            return await m_DbContext.AktivitetTyper.ToListAsync();
        }

        /// <summary>
        /// Async metod som returnerar Aktiviteter som tillhör en modul
        /// </summary>
        /// <param name="iModuleId">Id för den modul som vi vill ha aktivitetrna för</param>
        /// <returns>Task med List med Aktivitet som tillhör en sökt modul</returns>
        public async Task<List<Aktivitet>> GetModulesAktivitetAsync(int iModuleId)
        {
            return await m_DbContext.Aktiviteter
                .Include(m => m.Modul)
                .Include(a => a.AktivitetTyp)
                .Where(m => m.ModulId == iModuleId)
                .ToListAsync();
        }

        /// <summary>
        /// Metoden uppdaterar data om en Aktivitet
        /// </summary>
        /// <param name="aktivitet">Referense till Aktivitet som skall uppdateras</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till Aktivitet är null</exception>
        public void PutAktivitet(Aktivitet aktivitet)
        {
            if (aktivitet is null)
                throw new ArgumentNullException("AktivitetRepository.PutAktivitet. Referensen till Aktiviter är null");

            m_DbContext.Aktiviteter.Update(aktivitet);
        }

        /// <summary>
        /// Async metod som skapar en ny aktivitet
        /// </summary>
        /// <param name="aktivitet">Referense till Aktivitet som skall skapas</param>
        /// <exception cref="ArgumentNullException">Kastas om referensen till Aktivitet är null</exception>
        public async Task PostAktivitetAsync(Aktivitet aktivitet)
        {
            if (aktivitet is null)
                throw new ArgumentNullException("AktivitetRepository.PostAktivitetAsync. Referensen till Aktiviter är null");

            await m_DbContext.Aktiviteter.AddAsync(aktivitet);
        }

        /// <summary>
        /// Async metod som raderar en Aktivitet
        /// </summary>
        /// <param name="iAktivitetId">Id för Aktivitet som skall raderas</param>
        /// <returns>Task med antal raderade som raderades i databasen</returns>
        public async Task<int> DeleteAktivitetAsync(int iAktivitetId)
        {
            // Lite effektivare än att hämta objektet och sedan radera det
            return await m_DbContext.Database.ExecuteSqlRawAsync("DELETE from Aktiviteter WHERE id = @AktivitetId", 
                new SqlParameter("@AktivitetId", iAktivitetId));
        }

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        public async Task<bool> SaveAsync()
        {
            return (await m_DbContext.SaveChangesAsync()) >= 0;
        }
    }
}
