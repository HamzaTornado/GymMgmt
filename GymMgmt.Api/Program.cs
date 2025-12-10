using GymMgmt.Application;
using GymMgmt.Infrastructure;
using GymMgmt.Api.Middlewares.Exceptions;
using GymMgmt.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using GymMgmt.Api.Middlewares;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using GymMgmt.Api.Extensions;
using GymMgmt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymMgmt.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==========================================
            // 1. CONFIGURATION & SERVICES
            // ==========================================
            var appSettings = builder.Configuration.Get<AppSettings>();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

            string defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new EmptyConnectionStringException();

            builder.Services.AddApplicationServices(typeof(ApplicationDI).Assembly);
            builder.Services.AddInfrastructure(defaultConnection);

            // Ensure you created the 'SwaggerServiceExtensions.cs' file for this to work
            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // ==========================================
            // 2. SECURITY (CORS, AUTH, HEADERS)
            // ==========================================
            const string DefaultCorsPolicyName = "CorsPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, policy =>
                {
                    var origins = appSettings?.AppConfig?.CorsOrigins?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.TrimEnd('/'))
                        .ToArray() ?? Array.Empty<string>();

                    policy.WithOrigins(origins)
                          .SetIsOriginAllowedToAllowWildcardSubdomains() // Be careful with this in production
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var signingConfigurations = new JwtSigningConfigurations(appSettings!.JWT.Secret);
            builder.Services.AddSingleton(signingConfigurations);
            builder.Services.AddJwtAuthentication(appSettings.JWT, signingConfigurations);

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // CRITICAL: Tells the app it is behind Nginx
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var app = builder.Build();

            // ==========================================
            // 3. THE PIPELINE (MIDDLEWARE ORDER MATTERS)
            // ==========================================



            // 1. Handlers for Nginx/Proxy headers must be first
            app.UseForwardedHeaders();

            // 2. Swagger (Dev only or if you specifically want it in Prod)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 3. Routing & CORS
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);

            // 4. Custom Exception Handler
            app.UseExceptionsHandlingMiddleware();

            // 5. HTTPS Redirection
            // WARNING: Comment this out if you get "Too Many Redirects" errors behind Nginx
            // app.UseHttpsRedirection(); 

            // 6. Auth
            app.UseAuthentication();
            app.UseAuthorization();

            // 7. Map Endpoints
            app.MapControllers();

            // ==========================================
            // 4. DATABASE MIGRATION & SEEDING (Merged Block)
            // ==========================================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {// A. Migrate Database
                    var context = services.GetRequiredService<GymDbContext>();
                    await context.Database.MigrateAsync();

                    // B. Seed Roles (MUST BE FIRST)
                    var roleInitializer = services.GetRequiredService<IRoleInitializer>();
                    await roleInitializer.InitializeAsync();

                    // C. Seed Admin User 
                    var adminSeeder = services.GetRequiredService<IAdminSeeder>();
                    await adminSeeder.SeedAdminAsync();  

                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Database migrated and seeded successfully.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "CRITICAL ERROR: Database migration failed.");
                    // Optional: throw; // usage depends if you want the app to crash or continue running
                }
            }

            await app.RunAsync();
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