using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Identity
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAndUsersAsync(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger logger)
        {
            string[] roles = new[] { "Admin", "Author", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var r = new IdentityRole(role);
                    var res = await roleManager.CreateAsync(r);
                    logger.LogInformation("Created role {role} : {res}", role, res.Succeeded);
                }
            }

            // Create an admin user if not exists
            var adminEmail = configuration["Seed:AdminEmail"] ?? "admin@bookstore.local";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser { UserName = "admin", Email = adminEmail, EmailConfirmed = true };
                var create = await userManager.CreateAsync(adminUser, configuration["Seed:AdminPassword"] ?? "Admin123!");
                if (create.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Admin user created");
                }
                else
                {
                    logger.LogWarning("Failed to create admin user: {errors}", string.Join(", ", create.Errors.Select(e => e.Description)));
                }
            }

            // Optionally: create an Author and Customer seeded user similar way
            // ...
        }
    }
}
