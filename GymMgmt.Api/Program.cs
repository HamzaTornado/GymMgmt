
using Microsoft.AspNetCore.Identity;
using GymMgmt.Application;
using GymMgmt.Infrastructure;
using GymMgmt.Infrastructure.Exceptions;
using GymMgmt.Api.Middlewares.Exceptions;
using GymMgmt.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using GymMgmt.Api.Middlewares;


namespace GymMgmt.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var appSettings = builder.Configuration.Get<AppSettings>();
            bool isDevelopment = builder.Environment.IsDevelopment();
            string defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new EmptyConnectionStringException();


            // Add services to the container.

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
           
            builder.Services.AddApplicationServices(typeof(ApplicationDI).Assembly);
            builder.Services.AddInfrastructure(defaultConnection);

            const string DefaultCorsPolicyName = "CorsPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    // App:CorsOrigins in appsettings.json can contain more than one address separated
                    // by comma.
                    builder
                        .WithOrigins(
                            appSettings.AppConfig.CorsOrigins
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.TrimEnd('/'))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            var signingConfigurations = new JwtSigningConfigurations(appSettings.JWT.Secret);

            builder.Services.AddSingleton(signingConfigurations);

            builder.Services.AddJwtAuthentication(appSettings.JWT, signingConfigurations);

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthSystem.Api", Version = "v1" });
                //c.EnableAnnotations();
                //c.SchemaFilter<CustomSchemaFilters>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
            });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(DefaultCorsPolicyName);

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthorization();

            app.UseExceptionsHandlingMiddleware();
            app.MapControllers();

            app.Run();
        }
    }

    
}
public class AppSettings
{
    public AppConfig AppConfig { get; set; }
    public JwtSettings JWT { get; set; }
}
public class AppConfig
{
    public string CorsOrigins { get; set; }
    public string ApiVirtualPath { get; set; }
}