using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace GymMgmt.Infrastructure.Data
{
    public class GymDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string> , IUnitOfWork
    {
        public GymDbContext(DbContextOptions<GymDbContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
