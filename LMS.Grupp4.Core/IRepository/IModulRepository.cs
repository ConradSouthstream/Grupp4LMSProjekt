using LMS.Grupp4.Core.Entities;

namespace LMS.Grupp4.Core.IRepository
{
    public interface IModulRepository
    {
        /// <summary>
        /// Metoden returnerar sökt Modul eller null
        /// </summary>
        /// <param name="iModulId">ModulId för sökt Modul</param>
        /// <returns>Sökt Modul eller null</returns>
        Modul GetModulAsync(int iModulId);
    }
}
