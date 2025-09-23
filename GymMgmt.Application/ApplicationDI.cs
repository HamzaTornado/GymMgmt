using FluentValidation;
using GymMgmt.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GymMgmt.Application
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Assembly assembly)
        {
            // Register FluentValidation validators  
            services.AddValidatorsFromAssembly(
                assembly,
                ServiceLifetime.Scoped,
                null,
                includeInternalTypes: true);

            // Register MediatR  
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                // Pipeline behaviors (order matters!)  
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));       
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));        
            });

            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
