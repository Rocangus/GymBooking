using GymBooking19.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBooking19.Persistance
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider services, IConfiguration configuration)
        {
            var option = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var context = new ApplicationDbContext(option))
            {
                if (!context.Users.Any(u => u.UserName == "admin@Gymbokning.se"))
                {
                    
                }
            }
        }
    }
}
