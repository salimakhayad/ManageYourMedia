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
            // default settings
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            
            services.AddDbContext<MediaDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("MediaConnection"), b => b.MigrationsAssembly("MyMedia")));
           
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ElevatedRights", policy =>
                  policy.RequireRole("Administrator", "Gebruiker"));
            });

            services.AddIdentityCore<Profiel>(options => { })
                 .AddEntityFrameworkStores<MediaDbContext>()
                 .AddDefaultTokenProviders();

             services.AddScoped<IUserStore<Profiel>, ProfielUserStore>();

            //services.AddIdentityCore<string>(options => { });

            services.AddTransient<SignInManager<Profiel>>();
            services.AddTransient<MediaDbContext>();
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
