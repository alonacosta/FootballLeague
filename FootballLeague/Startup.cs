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
using System.Security.Claims;

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
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false; //disable carateres (no app real deve ser manter true)
                cfg.Password.RequiredUniqueChars = 0; // 1
                cfg.Password.RequireLowercase = false; // true
                cfg.Password.RequireUppercase = false; // true
                cfg.Password.RequireNonAlphanumeric = false; // true
                cfg.Password.RequiredLength = 6; // 8-12 
            })
                .AddEntityFrameworkStores<DataContext>();

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<SeedDb>();
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IBlobHelper, BlobHelper>();
            services.AddScoped<IConverterHelper, ConverterHelper>();

            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IFunctionRepository, FunctionRepository>();
            services.AddScoped<IStaffMemberRepository, StaffMemberRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            services.ConfigureApplicationCookie(options =>
            {
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
                            // Usu�rio n�o existe mais, rejeitar a sess�o
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
