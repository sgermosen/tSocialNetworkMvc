namespace Tetas.Web
{
    using Domain.Entities;
    using Helpers;
    using Infraestructure;
    using Infraestructure.Data;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Repositories.Contracts;
    using Repositories.Implementations;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            ConfigurePipeline(app);

            RunSeeding(app);

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddMyDependencies(configuration);

            services.AddDbContext<ApplicationDbContext>(cfg =>
            {
                var provider = configuration["DatabaseProvider"] ?? "Sqlite";
                if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                {
                    cfg.UseSqlServer(configuration.GetConnectionString("sGDatabaseCnn"));
                }
                else
                {
                    cfg.UseSqlite(configuration.GetConnectionString("SqliteCnn") ?? "Data Source=tetas.db");
                }
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
                {
                    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                    cfg.SignIn.RequireConfirmedEmail = false;
                    cfg.User.RequireUniqueEmail = true;
                    cfg.Password.RequireDigit = true;
                    cfg.Password.RequiredUniqueChars = 1;
                    cfg.Password.RequireLowercase = true;
                    cfg.Password.RequireNonAlphanumeric = false;
                    cfg.Password.RequireUppercase = true;
                    cfg.Password.RequiredLength = 8;
                    cfg.Lockout.MaxFailedAccessAttempts = 5;
                    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            EnsureSigningKey(configuration, builder.Environment.IsDevelopment());

            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Tokens:Issuer"],
                        ValidAudience = configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Tokens:Key"]))
                    };
                });

            services.AddTransient<SeedDb>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/NotAuthorized";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("ApiClients", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private static void ConfigurePipeline(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
                context.Response.Headers["X-XSS-Protection"] = "0";
                await next();
            });

            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/api"),
                branch => branch.UseStatusCodePagesWithReExecute("/error/{0}"));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("ApiClients");
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<Hubs.NotificationHub>("/hubs/notifications");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        private static void EnsureSigningKey(IConfiguration configuration, bool isDevelopment)
        {
            var configuredKey = configuration["Tokens:Key"];
            if (!string.IsNullOrWhiteSpace(configuredKey))
            {
                return;
            }

            if (!isDevelopment)
            {
                throw new InvalidOperationException(
                    "A signing key must be provided in configuration (Tokens:Key) for non-development environments.");
            }

            configuration["Tokens:Key"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private static void RunSeeding(WebApplication app)
        {
            var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();
                seeder.SeedAsync().Wait();
            }
        }
    }
}
