using LMS.Grupp4.Core.IRepository;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Repositories
{
    /// <summary>
    /// Unit of work med properties för att anropa repository
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Databas context
        /// </summary>
        private readonly ApplicationDbContext m_dbContext;

        /// <summary>
        /// Repository för Aktivitet
        /// </summary>
        public IAktivitetRepository AktivitetRepository { get; private set; }

        /// <summary>
        /// Repository för Modul
        /// </summary>
        public IModulRepository ModulRepository { get; private set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">Referense till context</param>
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            m_dbContext = applicationDbContext;

            AktivitetRepository = new AktivitetRepository(m_dbContext);
            ModulRepository = new ModulRepository(m_dbContext);
        }

        /// <summary>
        /// Metod som skall spara uppdaterad data i repositories
        /// </summary>
        /// <returns>Task</returns>
        public async Task CompleteAsync()
        {
            await ModulRepository.SaveAsync();
            await AktivitetRepository.SaveAsync();
        }
    }
}
