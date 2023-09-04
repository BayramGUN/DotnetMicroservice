using Microsoft.EntityFrameworkCore;
using PlatformService.Api.Models;

namespace PlatformService.Api.Data.Prepare;

public static class PrepareDataBase
{
    public static void PreparePopulation(IApplicationBuilder app, bool IsProduction)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            MigrateDb(serviceScope.ServiceProvider.GetService<AppDbContext>()!, IsProduction);
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!);
        }

    }

    private static void MigrateDb(AppDbContext context, bool isProduction)
    {
        if(isProduction)
        {
            Console.WriteLine("--> Attempting tp apply migrations");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"--> Could not run migrations: {exception.Message}");       
            }
        }
    }

    private static void SeedData(AppDbContext context)
    {
        if(!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding Data...");

            context.Platforms.AddRange(
                new Platform() {Name = "Dotnet", Publisher="Microsoft", Cost="Free"},
                new Platform() {Name = "SQL Server Express", Publisher="Microsoft", Cost="Free"},
                new Platform() {Name = "Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free"}
            );

            context.SaveChanges();
        }
        else
            Console.WriteLine("--> We already have data!");

    }
}