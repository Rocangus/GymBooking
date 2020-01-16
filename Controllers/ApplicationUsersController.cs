using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymBooking19.Data;
using GymBooking19.Models;
using GymBooking19.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymBooking19.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult CheckEmail(string email)
        {
            if(_context.Users.Any(u => u.Email == email))
            {
                return Json($"Email {email} is not available.");
            }
            return Json(true);
        }

        public IActionResult History()
        {
            var tuple = GetModelForUserWithGymClasses(historic: true);

            return View(tuple);
        }

        public IActionResult BookedClasses()
        {
            var tuple = GetModelForUserWithGymClasses(historic: false); ;

            return View(tuple);
        }

        private Tuple<ApplicationUser, IEnumerable<GymClassViewModel>> GetModelForUserWithGymClasses(bool historic)
        {
            List<ApplicationUserGymClass> results = GetUserWithBookedClasses(historic);

            

            Tuple<ApplicationUser, IEnumerable<GymClassViewModel>> tuple = new Tuple<ApplicationUser,
                                                                                     IEnumerable<GymClassViewModel>>(user, gymClasses);
            return tuple;
        }

        private static void PopulateGymClassViewModels(ApplicationUser user, List<GymClassViewModel> gymClasses)
        {
            foreach (var gymClass in user.AttendedClasses.Select(a => a.GymClass).ToList())
            {
                GymClassViewModel model = new GymClassViewModel
                {
                    Name = gymClass.Name,
                    Description = gymClass.Description,
                    StartTime = gymClass.StartTime,
                    Duration = gymClass.Duration.ToString(@"hh\:mm")
                };
                gymClasses.Add(model);
            }
        }

        public List<ApplicationUserGymClass> GetUserWithBookedClasses(bool history)
        {
            string userId = GetUserId();

            Task<List<ApplicationUserGymClass>> userTask;

            if(history)
            {
                userTask = _context.ApplicationUserGymClasses
                                     .Include(u => u.ApplicationUser)
                                     .Include(a => a.GymClass)
                                     .IgnoreQueryFilters()
                                     .Where(a => a.ApplicationUserId == userId && a.GymClass.StartTime < DateTime.UtcNow).ToListAsync();
            }
            else
            {
                userTask = _context.ApplicationUserGymClasses
                                     .Include(u => u.ApplicationUser)
                                     .Include(u => u.GymClass).Where(a => a.ApplicationUserId == userId).ToListAsync();
            }

            userTask.Wait();

            return userTask.Result;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }
    }
}
