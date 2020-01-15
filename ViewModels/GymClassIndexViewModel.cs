﻿using GymBooking19.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking19.ViewModels
{
    public class GymClassIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:yyy/MM/dd hh:mm}")]
        public DateTime StartTime { get; set; }
        public string Duration { get; set; }
        [Display(Name = "End Time")]
        public string EndTime { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ApplicationUserGymClass> AttendingMembers { get; set; }
    }
}
