using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBooking19.Data;
using GymBooking19.Models;
using Microsoft.AspNetCore.Identity;

namespace GymBooking19.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymClass.ToListAsync());
        }

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .Include(g => g.AttendingMembers).ThenInclude(a => a.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await _context.GymClass.FindAsync(id);
            _context.GymClass.Remove(gymClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id == null) return NotFound();

            var gymClass = await _context.GymClass
                .Include(g => g.AttendingMembers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (gymClass == null) return NotFound();

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null) return NotFound();

            var attendingMember = gymClass.AttendingMembers.FirstOrDefault(a => a.ApplicationUser.Id == user.Id);

            if (attendingMember == null)
            {
                attendingMember = new ApplicationUserGymClass
                {
                    ApplicationUser = user,
                    ApplicationUserId = user.Id,
                    GymClass = gymClass,
                    GymClassId = gymClass.Id
                };
                gymClass.AttendingMembers.Add(attendingMember);
                _context.Add(attendingMember);
                await _context.SaveChangesAsync();
                return View("Success");
            }
            gymClass.AttendingMembers.Remove(attendingMember);
            _context.Remove(attendingMember);
            await _context.SaveChangesAsync();
            return View("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        private bool GymClassExists(int id)
        {
            return _context.GymClass.Any(e => e.Id == id);
        }
    }
}
