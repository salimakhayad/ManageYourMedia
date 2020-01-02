using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMedia.Data;
using Microsoft.Extensions.Hosting;
using MyMedia.Core.MediaClasses;
using MyMedia.Core.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace MyMedia
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

          
            services.AddDbContextPool<MediaDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("MediaDb")
                ));
                   
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ElevatedRights", policy =>
                  policy.RequireRole("Administrator", "Gebruiker"));
            });

            services.AddIdentity<Profiel,IdentityRole>(options => { })
                 .AddEntityFrameworkStores<MediaDbContext>() // adds userstore and rolestore
                 .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<Profiel>, ProfielUserClaimsPrincipalFactory>();


            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");

            services.AddTransient<SignInManager<Profiel>>();
            services.AddTransient<UserManager<Profiel>>();
           
            services.AddTransient<IMyMediaService,MyMediaService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc((opt) => opt.EnableEndpointRouting = false);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseIdentity();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
