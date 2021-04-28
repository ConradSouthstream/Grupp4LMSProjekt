using LMS.Grupp4.Core.IRepository;
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
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        public async Task<bool> SaveAsync()
        {
            return (await m_DbContext.SaveChangesAsync()) >= 0;
        }
    }
}
