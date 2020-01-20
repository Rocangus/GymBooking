using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GymBooking19.Core.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndTime { get { return StartTime + Duration; } }
        [MaxLength(30)]
        public string Description { get; set; }
        public virtual ICollection<ApplicationUserGymClass> AttendingMembers { get; set; }

    }
}
