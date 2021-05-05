using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Repositories
{
    /// <summary>
    /// ElevRepository med metoder för att hantera Elever
    /// </summary>
    public class ElevRepository : IElevRepository
    {
        /// <summary>
        /// Databas context
        /// </summary>
        private readonly ApplicationDbContext m_DbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Referense till context</param>
        public ElevRepository(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        /// <summary>
        /// Metoden returnerar sökt Anvandare
        /// </summary>
        /// <param name="strElevId">Elevens id</param>
        /// <returns>Information om användaren eller null</returns>
        public async Task<Anvandare>GetAnvandareAsync(string strElevId)
        {
            return await m_DbContext.Anvandare.Where(id => id.Id == strElevId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async metod som returnerar Kurs som en elev deltar i samt information om användaren
        /// </summary>
        /// <param name="strElevId">Elevens id</param>
        /// <returns>En elevs kurs</returns>
        public async Task<Kurs> GetKursAsync(string strElevId)
        {
            Kurs kurs = null;

            // Hämta information om kurs och användaren
            var anvandareKurs = await m_DbContext.AnvandareKurser
                .Include(k => k.Kurs)
                .Where(e => e.AnvandareId == strElevId)
                .FirstOrDefaultAsync();

            if(anvandareKurs != null)
                kurs = anvandareKurs.Kurs;            

            return kurs;
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
