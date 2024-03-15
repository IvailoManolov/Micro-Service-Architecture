using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProduction);
            }
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> Attempting Migrations!");
                try
                {
                    context.Database.Migrate();
                }

                catch(Exception ex)
                {
                    Console.WriteLine($"--> Couldn't run migrations : {ex.Message}");
                }
            }

            // If context is empty, push data.
            if (!context.Platforms.Any())
            {
                Console.WriteLine(" --> Seeding Data ...");

                context.Platforms.AddRange(
                    new Platform(){ Name ="Dot Net", Publisher ="Microsoft", Cost="Free" },
                    new Platform() { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing", Cost = "Free" },
                    new Platform() { Name = "Visual Studio", Publisher = "Microsoft", Cost = "Free" }
                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine(" --> We already have data!");
            }
        }
    }
}
