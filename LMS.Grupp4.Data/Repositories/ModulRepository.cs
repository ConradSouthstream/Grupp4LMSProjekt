using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Grupp4.Data.Repositories
{
    /// <summary>
    /// ModulRepository med metoder för att hantera Modul
    /// </summary>
    public class ModulRepository : IModulRepository
    {
        private readonly ApplicationDbContext m_DbContext;

        /// <summary>
        /// TODO tmp under test
        /// </summary>
        private ICollection<Modul> m_lsModuler = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Referense till context</param>
        public ModulRepository(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;

            // TODO Tmp under utvecklingen
            m_lsModuler = SkapaModuler();
        }


        /// <summary>
        /// Metoden returnerar sökt Modul eller null
        /// </summary>
        /// <param name="iModulId">ModulId för sökt Modul</param>
        /// <returns>Sökt Modul eller null</returns>
        public Modul GetModulAsync(int iModulId)
        {
            return m_lsModuler.Where(m => m.ModulId == iModulId).FirstOrDefault();
        }


        /// <summary>
        /// TODO tmp under test
        /// </summary>
        /// <returns></returns>
        private List<Modul> SkapaModuler()
        {
            DateTime dtNow = DateTime.Now;
            m_lsModuler = new List<Modul>();

            Modul modul1 = new Modul
                { ModulId = 1, ModulNamn = "Module 1", Beskrivning = "Detta är modul 1", StartTid = dtNow.AddDays(-3), SlutTid = dtNow.AddDays(3) };

            m_lsModuler.Add(modul1);

            Modul modul2 = new Modul
                { ModulId = 2, ModulNamn = "Module 2", Beskrivning = "Detta är modul 2", StartTid = dtNow.AddDays(3), SlutTid = dtNow.AddDays(6) };

            m_lsModuler.Add(modul2);

            Modul modul3 = new Modul
                { ModulId = 3, ModulNamn = "Module 3", Beskrivning = "Detta är modul 3", StartTid = dtNow.AddDays(-6), SlutTid = dtNow.AddDays(-3) };

            m_lsModuler.Add(modul3);

            return m_lsModuler.ToList();
        }
    }
}
