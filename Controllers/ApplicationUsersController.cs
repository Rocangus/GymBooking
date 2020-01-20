using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymBooking19.Core.Models;
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
            var viewModels = GetModelsForUserWithGymClasses(historic: true);

            return View(viewModels);
        }

        public IActionResult BookedClasses()
        {
            var viewModels = GetModelsForUserWithGymClasses(historic: false); ;

            return View(viewModels);
        }

        private IEnumerable<GymClassViewModel> GetModelsForUserWithGymClasses(bool historic)
        {
            List<ApplicationUserGymClass> userClasses = GetUserWithBookedClasses(historic);

            var models = PopulateGymClassViewModels(userClasses);
            
            return models;
        }

        private static List<GymClassViewModel> PopulateGymClassViewModels(List<ApplicationUserGymClass> userClasses)
        {
            var gymClasses = new List<GymClassViewModel>();

            foreach (var userClass in userClasses)
            {
                var gymClass = userClass.GymClass;
                GymClassViewModel model = new GymClassViewModel
                {
                    Name = gymClass.Name,
                    Description = gymClass.Description,
                    StartTime = gymClass.StartTime,
                    Duration = gymClass.Duration.ToString(@"hh\:mm")
                };
                gymClasses.Add(model);
            }

            return gymClasses;
        }

        public List<ApplicationUserGymClass> GetUserWithBookedClasses(bool history)
        {
            string userId = GetUserId();

            Task<List<ApplicationUserGymClass>> userTask;

            if(history)
            {
                userTask = _context.ApplicationUserGymClasses
                                     .Include(a => a.GymClass)
                                     .IgnoreQueryFilters()
                                     .Where(a => a.ApplicationUserId == userId && a.GymClass.StartTime < DateTime.UtcNow).ToListAsync();
            }
            else
            {
                userTask = _context.ApplicationUserGymClasses
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
