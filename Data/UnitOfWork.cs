using GymBooking19.Core.Models;
using GymBooking19.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking19.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGymClassesRepository GymClasses { get; private set; }
        public IApplicationUserGymClassRepository UserGymClasses { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            GymClasses = new GymClassesRepository(context);
            UserGymClasses = new ApplicationUserGymClassRepository(context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
