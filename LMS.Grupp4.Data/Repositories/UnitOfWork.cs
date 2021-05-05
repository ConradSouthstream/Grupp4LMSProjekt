using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Hosting;
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
        private IHostingEnvironment _env;



        /// <summary>
        /// Repository för Aktivitet
        public IAktivitetRepository AktivitetRepository { get; private set; }

        /// <summary>
        /// Repository för Modul
        /// </summary>
        public IModulRepository ModulRepository { get; private set; }

        public IKursRepository KursRepository { get; private set; }
        public IDokumentRepository DokumentRepository { get; private set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="applicationDbContext">Referense till context</param>
        public UnitOfWork(ApplicationDbContext applicationDbContext,IHostingEnvironment env)
        {
            m_dbContext = applicationDbContext;
            _env = env;
            AktivitetRepository = new AktivitetRepository(m_dbContext);
            ModulRepository = new ModulRepository(m_dbContext);
            KursRepository = new KursRepository(m_dbContext);
            DokumentRepository = new DokumentRepository(_env,m_dbContext);
        }

        /// <summary>
        /// Metod som skall spara uppdaterad data i repositories
        /// </summary>
        /// <returns>Task</returns>
        public async Task CompleteAsync()
        {
            await ModulRepository.SaveAsync();
            await AktivitetRepository.SaveAsync();
            await KursRepository.SaveAsync();
            await DokumentRepository.SaveAsync();
        }
    }
}
