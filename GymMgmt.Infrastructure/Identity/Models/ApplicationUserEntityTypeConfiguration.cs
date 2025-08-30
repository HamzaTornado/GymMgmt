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
    /// EF Core configuration for ApplicationUser entity.
    /// </summary>
    internal class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            // Name properties
            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            // Refresh Token Security: Store only the hash
            builder.Property(u => u.RefreshTokenHash)
                .HasMaxLength(256) // PBKDF2 hash is ~88 chars in Base64
                .IsRequired(false);

            // Refresh token metadata
            builder.Property(u => u.RefreshTokenExpiryTime)
                .IsRequired(false);

            builder.Property(u => u.IsRefreshTokenRevoked)
                .IsRequired(false);

            // Index on refresh token hash for fast lookups
            builder.HasIndex(u => u.RefreshTokenHash)
                .IsUnique()
                .HasDatabaseName("IX_ApplicationUser_RefreshTokenHash");

            //// Soft delete support
            //builder.Property(u => u.IsDeleted)
            //    .HasDefaultValue(false);

            //builder.Property(u => u.DeletedAt)
            //    .IsRequired(false);

            // Optional: Add table comment
            builder.ToTable(t => t.HasComment("Application users with extended identity properties"));
        }
    }
}
