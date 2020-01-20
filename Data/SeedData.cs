using GymBooking19.Core.Models;
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
                    if (roleResult != IdentityResult.Success)
                    {
                        throw new Exception(string.Join("\n", roleResult.Errors));
                    }
                }

                if (!context.Users.Any(u => u.UserName == "admin@Gymbokning.se"))
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "admin@Gymbokning.se",
                        Email = "admin@Gymbokning.se",
                        FirstName = "Administrator",
                        LastName = "Foo",
                        TimeOfRegistration = DateTime.UtcNow
                    };
                    var addUserResult = await userManager.CreateAsync(admin, configuration["AdminPassword"]);

                    if (!addUserResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addUserResult.Errors));
                    }

                    var addToRoleResult = await userManager.AddToRoleAsync(admin, roleName);
                    
                    if (!addToRoleResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addToRoleResult.Errors));
                    }

                    await context.AddAsync(admin);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
