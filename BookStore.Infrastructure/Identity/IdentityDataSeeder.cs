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
            /*
            PSEUDOCODE / PLAN (detailed):

            1. Ensure required roles exist (Admin, Author, Customer) - already implemented.

            2. Create or ensure Admin user:
               - Read admin email and password from configuration with sensible defaults.
               - If user with that email doesn't exist:
                   - Create AppUser with UserName "admin", Email = adminEmail, EmailConfirmed = true.
                   - Create user with password; if succeeded add to "Admin" role.
                   - Log success or detailed errors.
               - If user exists but is not in "Admin" role, add to role and log.

            3. Create or ensure Author user:
               - Read author email and password from configuration keys "Seed:AuthorEmail" and "Seed:AuthorPassword"
                 with defaults "author@bookstore.local" and "Author123!".
               - If not exists, create AppUser with UserName "author", EmailConfirmed = true.
               - After creation, add to "Author" role and log.
               - If exists but not in role, add to role and log.

            4. Create or ensure Customer user:
               - Read customer email and password from configuration keys "Seed:CustomerEmail" and "Seed:CustomerPassword"
                 with defaults "customer@bookstore.local" and "Customer123!".
               - If not exists, create AppUser with UserName "customer", EmailConfirmed = true.
               - After creation, add to "Customer" role and log.
               - If exists but not in role, add to role and log.

            5. For all role-add operations, check results and log warnings with error descriptions when operations fail.

            Notes:
            - Use userManager.FindByEmailAsync to detect existing accounts.
            - Use userManager.IsInRoleAsync before adding to avoid duplicate role assignment attempts.
            - Keep behavior idempotent so seeding can be run multiple times safely.
            */

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
                    var addRole = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (addRole.Succeeded)
                        logger.LogInformation("Admin user created and added to Admin role");
                    else
                        logger.LogWarning("Admin user created but failed to add to role: {errors}", string.Join(", ", addRole.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogWarning("Failed to create admin user: {errors}", string.Join(", ", create.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    var addRole = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (addRole.Succeeded)
                        logger.LogInformation("Existing admin user added to Admin role");
                    else
                        logger.LogWarning("Failed to add existing admin user to Admin role: {errors}", string.Join(", ", addRole.Errors.Select(e => e.Description)));
                }
            }

            // Create or ensure an Author user
            var authorEmail = configuration["Seed:AuthorEmail"] ?? "author@bookstore.local";
            var authorUser = await userManager.FindByEmailAsync(authorEmail);
            if (authorUser == null)
            {
                authorUser = new AppUser { UserName = "author", Email = authorEmail, EmailConfirmed = true };
                var create = await userManager.CreateAsync(authorUser, configuration["Seed:AuthorPassword"] ?? "Author123!");
                if (create.Succeeded)
                {
                    var addRole = await userManager.AddToRoleAsync(authorUser, "Author");
                    if (addRole.Succeeded)
                        logger.LogInformation("Author user created and added to Author role");
                    else
                        logger.LogWarning("Author user created but failed to add to role: {errors}", string.Join(", ", addRole.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogWarning("Failed to create author user: {errors}", string.Join(", ", create.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(authorUser, "Author"))
                {
                    var addRole = await userManager.AddToRoleAsync(authorUser, "Author");
                    if (addRole.Succeeded)
                        logger.LogInformation("Existing author user added to Author role");
                    else
                        logger.LogWarning("Failed to add existing author user to Author role: {errors}", string.Join(", ", addRole.Errors.Select(e => e.Description)));
                }
            }

            // Create or ensure a Customer user
            var customerEmail = configuration["Seed:CustomerEmail"] ?? "customer@bookstore.local";
            var customerUser = await userManager.FindByEmailAsync(customerEmail);
            if (customerUser == null)
            {
                customerUser = new AppUser { UserName = "customer", Email = customerEmail, EmailConfirmed = true };
                var create = await userManager.CreateAsync(customerUser, configuration["Seed:CustomerPassword"] ?? "Customer123!");
                if (create.Succeeded)
                {
                    var addRole = await userManager.AddToRoleAsync(customerUser, "Customer");
                    if (addRole.Succeeded)
                        logger.LogInformation("Customer user created and added to Customer role");
                    else
                        logger.LogWarning("Customer user created but failed to add to role: {errors}", string.Join(", ", addRole.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogWarning("Failed to create customer user: {errors}", string.Join(", ", create.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(customerUser, "Customer"))
                {
                    var addRole = await userManager.AddToRoleAsync(customerUser, "Customer");
                    if (addRole.Succeeded)
                        logger.LogInformation("Existing customer user added to Customer role");
                    else
                        logger.LogWarning("Failed to add existing customer user to Customer role: {errors}", string.Join(", ", addRole.Errors.Select(e => e.Description)));
                }
            }

            // Optionally: create more seeded users or extend with claims/profiles
        }
    }
}
