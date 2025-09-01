using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Infrastructure.Data;
using GymMgmt.Infrastructure.Data.Members;
using GymMgmt.Infrastructure.Exceptions;
using GymMgmt.Infrastructure.Identity;
using GymMgmt.Infrastructure.Identity.Models;
using GymMgmt.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;


namespace GymMgmt.Infrastructure
{
    internal static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, bool isEnvironment)
        {
            
            if (string.IsNullOrEmpty(connectionString)) 
                throw new DatabaseConnectionException(nameof(connectionString));

            // Add the interceptors
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            // Register the DbContext with interceptors
            services.AddDbContext<GymDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseSqlServer(
                    connectionString,
                    sqloptions => sqloptions.MigrationsAssembly(typeof(GymDbContext).Assembly.FullName)
                );

                // Apply interceptors from service provider
                var interceptors = sp.GetServices<ISaveChangesInterceptor>().ToArray();
                options.AddInterceptors(interceptors);
            });

            // Register the IUnitOfWork interface
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<GymDbContext>());

            // Configure Identity to use the merged DbContext
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<GymDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // SignIn settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserManager, UserManager>();

            services.AddScoped<IMemberRepository,MemberRepository>();



            return services;
        }
    }
}
