using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        private ICollection<Aktivitet> m_lsAktiviteter = null;
        private List<AktivitetTyp> m_lsAktivitetTyper = null;


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dbContext">Referense till context</param>
        public AktivitetRepository(ApplicationDbContext dbContext)
        {
            m_DbContext = dbContext;

            // Todo. RADERA. BARA UNDER TEST
            m_lsAktiviteter = SkapaAktiviteter(10);
            m_lsAktivitetTyper = SkapaAktivitetTyper();
        }

        public List<Aktivitet> GetAktivitetAsync()
        {
            return m_lsAktiviteter.ToList();
        }

        /// <summary>
        /// Metoden returnerar sökt Aktivitet
        /// </summary>
        /// <param name="iAktivitetId">Id för sökt aktivitet</param>
        /// <returns>Sökt aktivitet eller null</returns>
        public Aktivitet GetAktivitetAsync(int iAktivitetId)
        {
            return m_lsAktiviteter.Where(i => i.AktivitetId == iAktivitetId).FirstOrDefault();
        }

        /// <summary>
        /// Metoden returnerar alla AktivitetTyper
        /// </summary>
        /// <returns>List med alla AktivitetTyper</returns>
        public List<AktivitetTyp> GetAktivitetTyperAsync()
        {
            return m_lsAktivitetTyper;
        }

        /// <summary>
        /// Metoden returnerar Aktivitet somk tillhör en modul
        /// </summary>
        /// <param name="iModuleId">Id för den modul som vi vill ha aktivitetrna för</param>
        /// <returns>List med Aktivitet som tillhör en sökt modul</returns>
        public List<Aktivitet> GetModulesAktivitetAsync(int iModuleId)
        {
            return m_lsAktiviteter.Where(m => m.ModulId == iModuleId).ToList();
        }


        /// <summary>
        /// Metoden uppdaterar data om en Aktivitet
        /// </summary>
        /// <param name="aktivitet">Referense till Aktivitet som skall uppdateras</param>
        public void PutAktivitetAsync(Aktivitet aktivitet)
        {
            // TODO
            throw new NotImplementedException("AktivitetRepository -> PutAktivitetAsync");
        }


        /// <summary>
        /// TODO UNDER TEST
        /// Skall raderas
        /// </summary>
        /// <returns></returns>
        public List<AktivitetTyp> SkapaAktivitetTyper()
        {
            List<AktivitetTyp> lsAktivitetTyper = new List<AktivitetTyp>();
            AktivitetTyp aktivitetTyp = null;

            for(int i = 1; i <= 5; i++)
            {
                aktivitetTyp = new AktivitetTyp();
                aktivitetTyp.AktivitetTypId = i;
                aktivitetTyp.AktivitetTypNamn = "AktiviteTyp " + i;
                lsAktivitetTyper.Add(aktivitetTyp);
            }

            return lsAktivitetTyper;
        }


        /// <summary>
        /// TODO Skapa data under test
        /// Skall inte användas
        /// </summary>
        /// <returns></returns>
        private List<Aktivitet> SkapaAktiviteter(int iAntalAktiviteter)
        {
            List<Aktivitet> lsAktiviteter = new List<Aktivitet>(iAntalAktiviteter);
            Aktivitet aktivitet = null;
            AktivitetTyp aktivitetTyp = null;
            Dokument dokument = null;
            DateTime dtNow = DateTime.Now;

            for (int i = 1; i <= iAntalAktiviteter; i++)
            {
                // TODO. Skapa testdatas
                aktivitet = new Aktivitet();
                aktivitet.AktivitetId = i;
                aktivitet.AktivitetNamn = "Aktivitet " + i;
                aktivitet.AktivitetTyp = new AktivitetTyp
                    { AktivitetTypId = 1, AktivitetTypNamn = "Aktivitet " + i };
                aktivitet.AktivitetTypId = 1;
                aktivitet.Beskrivning = "Detta är Aktivitet " + i;
                aktivitet.Modul = new Modul 
                    { ModulId = 1, ModulNamn = "Module 1", Beskrivning = "Detta är modul 1", StartTid = dtNow.AddDays(-3), SlutTid = dtNow.AddDays(3) };
                aktivitet.ModulId = 1;

                if (i == 1)
                {
                    aktivitet.StartTid = dtNow.AddDays(-1);
                    aktivitet.SlutTid = dtNow.AddDays(1);
                }
                else if(i == 2)
                {
                    aktivitet.StartTid = dtNow.AddDays(-3);
                    aktivitet.SlutTid = dtNow.AddDays(-2);
                }
                else
                {
                    aktivitet.StartTid = dtNow.AddDays(i);
                    aktivitet.SlutTid = dtNow.AddDays(i+1);
                }


                lsAktiviteter.Add(aktivitet);
            }

            return lsAktiviteter;
        }


        /// <summary>
        /// Async metod som sparar ändringar
        /// </summary>
        /// <returns>true om några ändringar sparas. Annars returneras false</returns>
        //public async Task<bool> SaveAsync()
        public bool SaveAsync()
        {
            // TODO Ändra SaveAsync to context
            //return (await m_DbContext.SaveChangesAsync()) >= 0;
            return true;
        }
    }
}
