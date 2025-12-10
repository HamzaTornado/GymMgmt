using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Identity
{
    public interface IAdminSeeder
    {
        Task SeedAdminAsync();
    }
    public class AdminSeeder : IAdminSeeder
    {
        // If you have a custom class like 'ApplicationUser', replace 'IdentityUser' with it here
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AdminSeeder> _logger;

        public AdminSeeder(UserManager<IdentityUser> userManager, ILogger<AdminSeeder> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task SeedAdminAsync()
        {
            var adminEmail = "admin@gym.com";

            // 1. Check if the admin already exists
            var existingUser = await _userManager.FindByEmailAsync(adminEmail);

            if (existingUser == null)
            {
                // 2. Create the User object
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // 3. Create the user in the DB with a default password
                // TIP: In production, consider loading this password from appsettings or Environment Variables
                var createResult = await _userManager.CreateAsync(newAdmin, "AdminPassword!123");

                if (createResult.Succeeded)
                {
                    _logger.LogInformation("Default Admin user created successfully.");

                    // 4. Assign the 'Admin' role
                    // This relies on RoleInitializer running BEFORE this seeder
                    await _userManager.AddToRoleAsync(newAdmin, "Admin");
                    _logger.LogInformation("Admin role assigned to the default user.");
                }
                else
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    _logger.LogError($"Failed to create Admin user: {errors}");
                }
            }
            else
            {
                _logger.LogInformation("Admin user already exists. Seeding skipped.");
            }
        }
    }
}
