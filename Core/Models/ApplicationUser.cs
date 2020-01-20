using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymBooking19.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get => FirstName + " " + LastName; }
        [Required]
        public DateTime TimeOfRegistration { get; set; }
        public virtual ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
