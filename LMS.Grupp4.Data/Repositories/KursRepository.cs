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
    public class KursRepository : IKursRepository
    {
        /// <summary>
        /// Databas context
        /// </summary>
        private readonly ApplicationDbContext m_DbContext;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Referense till context</param>
        public KursRepository(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }

        public async Task<Kurs> GetKursAsync(int? iKursId)
        {
            return await m_DbContext.Kurser.FirstOrDefaultAsync(k=>k.Id==iKursId);
        }

        public async Task<IEnumerable<Kurs>> GetAllKurserAsync()
        {
            return await m_DbContext.Kurser.ToListAsync();

        }

        public bool KursExists(int? ikursId)
        {
            return m_DbContext.Kurser.Any(K=>K.Id==ikursId);
        }

        public async Task PostKursAsync(Kurs Kurs)
        {
            await m_DbContext.Kurser.AddAsync(Kurs);
        }

        public void PutKurs(Kurs Kurs)
        {
            m_DbContext.Update(Kurs);
        }

        public async Task<bool> SaveAsync()
        {
            return (await m_DbContext.SaveChangesAsync() >= 0);
        }

        public void RemoveKursAsync(Kurs kurs)
        {
            m_DbContext.Remove(kurs);
        }
    }
}
