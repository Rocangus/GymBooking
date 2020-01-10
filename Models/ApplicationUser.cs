using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GymBooking19.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
