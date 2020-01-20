using GymBooking19.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymBooking19.Core.Repositories;
using GymBooking19.Data;

namespace GymBooking19.Core.Models
{
    public class GymClassesRepository : IGymClassesRepository
    {
        private ApplicationDbContext _context;

        public GymClassesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(GymClass gymClass)
        {
            _context.Add(gymClass);
        }

        public void Update(GymClass gymClass)
        {
            _context.Update(gymClass);
        }

        public async Task Remove(int id)
        {
            var gymClass = await _context.GymClass.FindAsync(id);
            _context.GymClass.Remove(gymClass);
        }

        public async Task<GymClass> GetClassByIdAsync(int? id)
        {
            return await _context.GymClass
                            .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<GymClass> GetClassWithAttendingMembersAsync(int? id)
        {
            return await _context.GymClass
                .Include(g => g.AttendingMembers).ThenInclude(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<GymClass>> GetAllWithUsersAsync(bool historic)
        {
            List<GymClass> classes;
            if (historic)
            {
                classes = await _context.GymClass.Include(g => g.AttendingMembers).ThenInclude(a => a.ApplicationUser).IgnoreQueryFilters().ToListAsync();
            }
            else
            {
                classes = await _context.GymClass.Include(g => g.AttendingMembers).ThenInclude(a => a.ApplicationUser).ToListAsync();
            }

            return classes;
        }



        public bool GetAny(int id)
        {
            return _context.GymClass.Any(e => e.Id == id);
        }
    }
}
