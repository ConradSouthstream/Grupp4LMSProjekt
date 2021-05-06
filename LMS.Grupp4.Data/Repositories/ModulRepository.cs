using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Repositories
{
    /// <summary>
    /// ModulRepository med metoder för att hantera Modul
    /// </summary>
    public class ModulRepository : IModulRepository
    {
        private readonly ApplicationDbContext m_DbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Referense till context</param>
        public ModulRepository(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public void AddModul(Modul modul)
        {
            m_DbContext.Moduler.Add(modul);

        }

        /// <summary>
        /// Async metod som returnerar Moduler som tillhör en kurs
        /// </summary>
        /// <param name="ikursId">Id för den kurs som vi vill ha modulerna för</param>
        /// <returns>Task med IEnumerable till Moduler som tillhör en sökt kurs</returns>
        public async Task<IEnumerable<Modul>> GetKursModulerAsync(int ikursId)
        {
            return await m_DbContext.Moduler
                .Where(m => m.KursId == ikursId)
                .ToListAsync();
        }

        /// <summary>
        /// Async metod som returnerar en lista med Moduler som tillhör en kurs. Inkluderar även en moduls aktivitet
        /// </summary>
        /// <param name="iKursId">Kursens id</param>
        /// <returns>Lista med Moduler som tillhör en kurs inklusive modulens aktiviteter</returns>
        public async Task<IEnumerable<Modul>> GetKursModulerIncludeAktivitetAsync(int iKursId)
        {
            return await m_DbContext.Moduler
                .Include(k => k.Kurs)
                .Include(a => a.Aktiviteter)
                .ThenInclude(a => a.AktivitetTyp)
                .Where(m => m.KursId == iKursId)
                .ToListAsync();
        }

        /// <summary>
        /// Async metod som returnerar sökt Modul eller null
        /// </summary>
        /// <param name="iModulId">ModulId för sökt Modul</param>
        /// <returns>Task med sökt Modul eller null</returns>
        public async Task<Modul> GetModulAsync(int iModulId)
        {
            return await m_DbContext.Moduler
                .Where(m => m.Id == iModulId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async metod som returnerar sökt Modul inklusive Kurs eller null
        /// </summary>
        /// <param name="iModulId">ModulId för sökt Modul</param>
        /// <returns>Task med sökt Modul inklusive Kurs eller null</returns>
        public async Task<Modul> GetModulWithKursAsync(int iModulId)
        {
            return await m_DbContext.Moduler
                .Include(k => k.Kurs).
                Where(m => m.Id == iModulId)
                .FirstOrDefaultAsync();
        }

        public async Task<Modul> GetModulByKurs(int kursId, int iModulId)
        {
            return await m_DbContext.Moduler.Where(m => m.KursId == kursId && m.Id == iModulId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        public async Task<bool> SaveAsync()
        {
            return (await m_DbContext.SaveChangesAsync()) >= 0;
        }

        public void UpdateModul(Modul modul)
        {
            m_DbContext.Moduler.Update(modul);
        }
    }
}