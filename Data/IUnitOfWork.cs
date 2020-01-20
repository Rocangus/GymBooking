using System.Threading.Tasks;
using GymBooking19.Core.Repositories;

namespace GymBooking19.Data
{
    public interface IUnitOfWork
    {
        IGymClassesRepository GymClasses { get; }
        IApplicationUserGymClassRepository UserGymClasses { get; }

        Task CompleteAsync();
    }
}