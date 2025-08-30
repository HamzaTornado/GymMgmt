using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Identity.Models
{
    /// <summary>
    /// Extended Identity user with custom properties and refresh token support.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Computed full name from first and last name.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();

        // Refresh Token Security: Store only the HASH
        public string? RefreshTokenHash { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        public bool? IsRefreshTokenRevoked { get; set; }

        //// Optional: Soft delete support
        //public bool IsDeleted { get; set; }
        //public DateTimeOffset? DeletedAt { get; set; }
    }
}
