using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GymBooking19.Data;
using Microsoft.AspNetCore.Identity;
using GymBooking19.Models;
using GymBooking19.Core.Models;

namespace GymBooking19
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services, services.GetRequiredService<IConfiguration>(),
                        services.GetRequiredService<UserManager<ApplicationUser>>(),
                        services.GetRequiredService<RoleManager<IdentityRole>>()).Wait();
                } 
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e.Message, "Seed failed");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
