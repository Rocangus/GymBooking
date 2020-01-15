using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymBooking19.Models;
using Microsoft.AspNetCore.Identity;

namespace GymBooking19.Extensions
{
    public static class UserManagerExtensions
    {
        public static string GetFullName(UserManager<ApplicationUser> userManager, string Id)
        {
            return userManager.FindByIdAsync(Id).Result.FullName;
        }
    }
}
