using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Async metod som returnerar sökt Modul eller null
        /// </summary>
        /// <param name="iModulId">ModulId för sökt Modul</param>
        /// <returns>Task med sökt Modul eller null</returns>
        public async Task<Modul> GetModulAsync(int iModulId)
        {
            return await m_DbContext.Moduler.Where(m => m.Id == iModulId).FirstOrDefaultAsync();
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