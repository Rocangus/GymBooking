using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymBooking19.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
        [Required]
        public DateTime TimeOfRegistration { get; set; }
        public virtual ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
