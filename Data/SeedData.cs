using GymBooking19.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking19.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider services, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var option = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var context = new ApplicationDbContext(option))
            {
                string roleName = "Admin";
                IdentityResult roleResult;
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                if (!context.Users.Any(u => u.UserName == "admin@Gymbokning.se"))
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "admin@Gymbokning.se",
                        Email = "admin@Gymbokning.se"
                    };
                    await userManager.CreateAsync(admin, configuration["AdminPassword"]);
                    await userManager.AddToRoleAsync(admin, roleName);
                    await context.AddAsync(admin);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
