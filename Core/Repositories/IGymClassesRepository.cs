using System.Collections.Generic;
using System.Threading.Tasks;
using GymBooking19.Core.Models;
using GymBooking19.Models;

namespace GymBooking19.Core.Repositories
{
    public interface IGymClassesRepository
    {
        void Add(GymClass gymClass);
        Task<List<GymClass>> GetAllWithUsersAsync(bool historic);
        bool GetAny(int id);
        Task<GymClass> GetClassByIdAsync(int? id);
        Task<GymClass> GetClassWithAttendingMembersAsync(int? id);
        Task Remove(int id);
        void Update(GymClass gymClass);
    }
}