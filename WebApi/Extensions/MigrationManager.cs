using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch
                    {
                        throw new Exception("Api Migration failed");
                    }
                }
                using (var appContext = scope.ServiceProvider.GetRequiredService<IdentityDataContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();

                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                        var user = userManager.FindByNameAsync("Admin").Result!;

                        var result = userManager.AddToRoleAsync(user, "Admin").Result;

                    }
                    catch
                    {
                        throw new Exception("Identity Migration failed");
                    }
                }
            }
            return app;
        }
    }
}
