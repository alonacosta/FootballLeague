using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Filters;
using FootballLeague.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Vereyon.Web;

namespace FootballLeague
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH9fcXRQRmZfVEV+X0o=");

            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false; //disable carateres (no app real deve ser manter true)
                cfg.Password.RequiredUniqueChars = 0; // 1
                cfg.Password.RequireLowercase = false; // true
                cfg.Password.RequireUppercase = false; // true
                cfg.Password.RequireNonAlphanumeric = false; // true
                cfg.Password.RequiredLength = 6; // 8-12 
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddFlashMessage();

            services.AddTransient<SeedDb>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IBlobHelper, BlobHelper>();
            services.AddScoped<IConverterHelper, ConverterHelper>();
            services.AddScoped<IMailHelper, MailHelper>();

            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IFunctionRepository, FunctionRepository>();
            services.AddScoped<IStaffMemberRepository, StaffMemberRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IRoundRepository, RoundRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IIncidentRepository, IncidentRepository>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/NotAuthorized";

                options.Events.OnValidatePrincipal = async context =>
                {
                    var userPrincipal = context.Principal;
                    if (userPrincipal != null)
                    {
                        var userId = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();

                        var user = await userManager.FindByIdAsync(userId);
                        if (user == null)
                        {
                            // User don't exist more, recuse a session
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync();
                        }
                    }
                };
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<UserProfileImageFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();            

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "matchesRoute",
                //    pattern: "Matches/{action=Index}/{id?}",
                //    defaults: new { controller = "Matches" }
                //);

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
