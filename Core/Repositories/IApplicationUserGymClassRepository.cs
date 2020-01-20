using System.Threading.Tasks;
using GymBooking19.Core.Models;
using GymBooking19.Models;

namespace GymBooking19.Core.Repositories
{
    public interface IApplicationUserGymClassRepository
    {
        Task AddAsync(ApplicationUserGymClass userClass);
        void Remove(ApplicationUserGymClass userClass);
    }
}