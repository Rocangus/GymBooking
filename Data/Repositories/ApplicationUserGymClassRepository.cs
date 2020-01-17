using GymBooking19.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking19.Data.Repositories
{
    public class ApplicationUserGymClassRepository : 
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserGymClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ApplicationUserGymClass userClass)
        {
            await _context.AddAsync(userClass);
        }

        public void Remove(ApplicationUserGymClass userClass)
        {
            _context.Remove(userClass);
        }
    }
}
