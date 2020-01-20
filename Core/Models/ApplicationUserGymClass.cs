using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking19.Core.Models
{
    public class ApplicationUserGymClass
    {

        public string ApplicationUserId { get; set; }
        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }
        public int GymClassId { get; set; }
        // Navigation property
        public GymClass GymClass { get; set; }
    }
}
