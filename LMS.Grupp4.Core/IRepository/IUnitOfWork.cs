using System.Threading.Tasks;

namespace LMS.Grupp4.Core.IRepository
{
    /// <summary>
    /// Interface för Unit of work objektet
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// AktivitetRepository
        /// </summary>
        IAktivitetRepository AktivitetRepository { get; }

        /// <summary>
        /// Metod som skall spara uppdaterad data i repositories
        /// </summary>
        /// <returns>Task</returns>
        Task CompleteAsync();
    }
}
