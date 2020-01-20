using GymBooking19.Core.Models;
using GymBooking19.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GymBooking19.ViewComponents
{
    public class BookViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BookViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke(object arguments)
        {
            GymClass gymClass = arguments as GymClass;
            var userId = _userManager.GetUserId(HttpContext.User);
            var model = new Tuple<GymClass, string>(gymClass, userId);
            return View(model);
        }
    }
}
