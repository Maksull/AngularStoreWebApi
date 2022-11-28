using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.Database
{
    public sealed class IdentitySeedData
    {
        private const string _adminUser = "Admin";
        private const string _adminPassword = "Secret123$";
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            IdentityDataContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityDataContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<IdentityUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser? user = await userManager.FindByNameAsync(_adminUser);

            if (user == null)
            {
                user = new IdentityUser(_adminUser);
                user.Email = "admin@example.com";
                user.PhoneNumber = "111-222";
                await userManager.CreateAsync(user, _adminPassword);
            }
        }
    }
}
