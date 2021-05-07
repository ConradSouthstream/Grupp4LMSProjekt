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
    class DokumentTypRepository : IDokumentTypRepository
    {
        private readonly ApplicationDbContext m_DbContext;

        public DokumentTypRepository(ApplicationDbContext context)
        {
            m_DbContext = context;
        }

        public async Task CreateDokumentTypAsync(DokumentTyp dokumentTyp)
        {
           await m_DbContext.DokumentTyper.AddAsync(dokumentTyp);
        }

        public  bool DokumentTypExists(int? idokumentTypId)
        {
            return m_DbContext.DokumentTyper.Any(d=>d.Id== idokumentTypId);
        }

        public async Task<IEnumerable<DokumentTyp>> GetAllDokumentTyperAsync()
        {
            return await m_DbContext.DokumentTyper.ToListAsync();
        }

        public async  Task<DokumentTyp> GetDokumentTypAsync(int? idokumentTypId)
        {
            return await m_DbContext.DokumentTyper.FirstOrDefaultAsync(d=>d.Id==idokumentTypId);
        } 

        public void RemoveDokumentTypAsync(DokumentTyp dokumentTyp)
        {
            m_DbContext.Remove(dokumentTyp);
        }

        public async Task<bool> SaveAsync()
        {
            return (await m_DbContext.SaveChangesAsync() >= 0);
        }

        public void UpdateDokumentTyp(DokumentTyp dokumentTyp)
        {
            m_DbContext.Update(dokumentTyp);
        }
    }
}
