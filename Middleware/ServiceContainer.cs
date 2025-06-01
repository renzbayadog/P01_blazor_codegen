using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Identity.Client;
using System.Text;
using codegeneratorlib.Helpers;
using RenzGrandWeddingblazor.ph.Data.Entities;
using RenzGrandWeddingblazor.ph.Data.Repositories;

namespace RenzGrandWeddingblazor.ph.Middleware
{

    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastractureService(this IServiceCollection services,
                                                                       IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<db_ab9d6a_dbrenzContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")),ServiceLifetime.Scoped);

            //services.AddIdentity<User, Role>(cfg =>
            //{
            //    cfg.User.RequireUniqueEmail = true;
            //    cfg.Password.RequiredLength = 6;
            //    cfg.Password.RequireNonAlphanumeric = false;
            //    cfg.Password.RequireLowercase = false;
            //    cfg.Password.RequireUppercase = false;
            //    cfg.Password.RequireDigit = false;

            //}).AddEntityFrameworkStores<AppDB1Context>()
            //  .AddSignInManager()
            //  .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = "/Account/Login";
                opts.AccessDeniedPath = "/Account/AccessDenied";
            });

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
            //    options.DefaultScheme = IdentityConstants.ExternalScheme;
            //});
            //services.AddAuthentication()
            //     .AddCookie()
            //     .AddJwtBearer(cfg =>
            //     {
            //         cfg.TokenValidationParameters = new TokenValidationParameters()
            //         {
            //             ValidIssuer = _config["Tokens:Issuer"],
            //             ValidAudience = _config["Tokens:Audience"],
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
            //         };
            //     });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddAuthorizationBuilder()
                    .AddPolicy("AdministrationPolicy", adp =>
                    {
                        adp.RequireAuthenticatedUser();
                        adp.RequireRole("Admin", "Manager");
                    }).AddPolicy("UserPolicy", adp =>
                    {
                        adp.RequireAuthenticatedUser();
                        adp.RequireRole("User");
                    });

            services.AddCascadingAuthenticationState();
            //services.AddScoped<Data.IDbContextFactory<AppDB1Context>, DbContextFactory<AppDB1Context>>();


            services.AddTransient<AppHelper>();
            //services.AddTransient<seeder>();
            //services.AddTransient<AccessRoleHelper>();
            //services.AddTransient<AuthServices>();
            //services.AddScoped<IAccount, Account>();
            //services.AddScoped<IFtpManager, FtpManager>();
            //services.AddScoped<IMenuManager, MenuManager>();
            //services.AddScoped<IEmailManager, EmailManager>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreatePulloutHandler).Assembly));

            return services;
        }
    }
}