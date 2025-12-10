using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Subscriptions;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Members;
using GymMgmt.Domain.Entities.Plans;
using GymMgmt.Infrastructure.Data;
using GymMgmt.Infrastructure.Data.ClubSettingsConfig;
using GymMgmt.Infrastructure.Data.Members;
using GymMgmt.Infrastructure.Data.Plans;
using GymMgmt.Infrastructure.Data.Subscriptions;
using GymMgmt.Infrastructure.Exceptions;
using GymMgmt.Infrastructure.Identity;
using GymMgmt.Infrastructure.Identity.Models;
using GymMgmt.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace GymMgmt.Infrastructure
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            
            if (string.IsNullOrEmpty(connectionString)) 
                throw new DatabaseConnectionException();

            // Add the interceptors
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            // Register the DbContext with interceptors
            services.AddDbContext<GymDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.MigrationsAssembly(typeof(GymDbContext).Assembly.FullName));

                // Apply interceptors
                var interceptors = serviceProvider.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptors);
            });

            // Scoped is recommended for connection factory
            services.AddScoped<ISqlConnectionFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<SqlConnectionFactory>>();
                return new SqlConnectionFactory(connectionString!, logger);
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

            //Identity services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddTransient<IRoleInitializer, RoleInitializer>();
            services.AddTransient<IAdminSeeder, AdminSeeder>();
            // Repositories
            services.AddScoped<IMemberRepository,MemberRepository>();
            services.AddScoped<IClubSettingsRepository,ClubSettingsRepository>();
            services.AddScoped<IMemberShipPlanRepository, MemberShipPlanRepository>();

            return services;
        }
    }
}
