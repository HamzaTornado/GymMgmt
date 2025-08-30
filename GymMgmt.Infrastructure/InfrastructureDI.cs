using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Infrastructure.Data;
using GymMgmt.Infrastructure.Exceptions;
using GymMgmt.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            return services;
        }
    }
}
