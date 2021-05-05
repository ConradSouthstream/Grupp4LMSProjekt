using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Repositories
{
    /// <summary>
    /// AnvandareRepository med metoder för att hantera anvandare
    /// </summary>
    public class AnvandareRepository : IAnvandareRepository
    {
        /// <summary>
        /// Databas context
        /// </summary>
        private readonly ApplicationDbContext m_DbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Referense till context</param>
        public AnvandareRepository(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        /// <summary>
        /// Async metod som returnerar alla anvandare på en kurs
        /// </summary>
        /// <param name="iKursId">Kursens id</param>
        /// <returns>Lis med användare på en kurs</returns>
        public async Task <List<Anvandare>> GetAnvandarePaKursAsync(int iKursId)
        {
            var anvandareKurs = await m_DbContext.AnvandareKurser
                .Include(a => a.Anvandare)
                .Where(k => k.KursId == iKursId)
                .Select(a => a.Anvandare)
                .ToListAsync();

            return anvandareKurs;
        }
    }
}
