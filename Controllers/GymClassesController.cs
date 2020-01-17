using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymBooking19.Data;
using GymBooking19.Data.Repositories;
using GymBooking19.Models;
using GymBooking19.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

namespace GymBooking19.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GymClassesRepository gymClassesRepository;
        private readonly ApplicationUserGymClassRepository userClassRepository;
        private readonly UnitOfWork unitOfWork;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            gymClassesRepository = new GymClassesRepository(context);
            userClassRepository = new ApplicationUserGymClassRepository(context);
            unitOfWork = new UnitOfWork(context);
        }

        // GET: GymClasses
        public async Task<IActionResult> Index(bool historic = false)
        {
            List<GymClass> classes = await gymClassesRepository.GetAllWithUsersAsync(historic);

            List<GymClassViewModel> models = new List<GymClassViewModel>();


            foreach (var gymClass in classes)
            {
                GymClassViewModel model = new GymClassViewModel
                {
                    Id = gymClass.Id,
                    Name = gymClass.Name,
                    StartTime = gymClass.StartTime,
                    Duration = gymClass.Duration.ToString(@"hh\:mm"),
                    Description = gymClass.Description,
                    AttendingMembers = gymClass.AttendingMembers,
                    EndTime = gymClass.EndTime
                };
                models.Add(model);
            }

            return View(models);
        }

        

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GymClass gymClass = await gymClassesRepository.GetClassWithAttendingMembersAsync(id);

            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                gymClassesRepository.Add(gymClass);
                await unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await gymClassesRepository.GetClassByIdAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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
                    gymClassesRepository.Update(gymClass);
                    await unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!gymClassesRepository.GetAny(gymClass.Id))
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GymClass gymClass = await gymClassesRepository.GetClassByIdAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await gymClassesRepository.Remove(id);
            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (id == null) return NotFound();

            var gymClass = await gymClassesRepository.GetClassWithAttendingMembersAsync(id);

            if (gymClass == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound();

            var attendingMember = gymClass.AttendingMembers.FirstOrDefault(a => a.ApplicationUser.Id == userId);

            if (attendingMember == null)
            {
                attendingMember = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = gymClass.Id
                };
                await userClassRepository.AddAsync(attendingMember);
                await unitOfWork.CompleteAsync();
                return View(nameof(Index));
            }
            
            await unitOfWork.CompleteAsync();
            return View(nameof(Index));
        }

        

        public IActionResult Success()
        {
            return View();
        }
    }
}
