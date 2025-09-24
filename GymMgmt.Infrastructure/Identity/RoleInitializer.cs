using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Identity
{
    public interface IRoleInitializer
    {
        Task InitializeAsync();
    }

    public class RoleInitializer : IRoleInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleInitializer> _logger;

        public RoleInitializer(RoleManager<IdentityRole> roleManager, ILogger<RoleInitializer> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            var roles = new[] { "Admin", "Manager", "Member" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    _logger.LogInformation("Created role: {Role}", role);
                }
            }

            _logger.LogInformation("Role initialization completed");
        }
    }
}
