using GymBooking19.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GymBooking19.Core.Models;

namespace GymBooking19.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserGymClass>().HasKey(t => new { t.ApplicationUserId, t.GymClassId });

            modelBuilder.Entity<GymClass>().HasQueryFilter(g => g.StartTime > System.DateTime.Now);
        }

        public DbSet<GymClass> GymClass { get; set; }
        public DbSet<ApplicationUserGymClass> ApplicationUserGymClasses { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
