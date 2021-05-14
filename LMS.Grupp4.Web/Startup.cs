using ClientNotifications;
using ClientNotifications.ServiceExtensions;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Data;
using LMS.Grupp4.Data.Hubs;
using LMS.Grupp4.Data.MapperProfiles;
using LMS.Grupp4.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;
using NToastNotify;
using System.IO;

namespace LMS.Grupp4.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar =true,
                PositionClass=ToastPositions.TopRight,
                PreventDuplicates=true,               
            } );

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<Anvandare>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
                //options.Password.RequireDigit = false;

            })
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddHttpClient("KursLitteraturWebApiHttpClient", c =>
            {
                c.BaseAddress = new Uri(Configuration["UrlWebApi:Url"]);
                //c.DefaultRequestHeaders.Add("Content-Type", "application/json");
                c.DefaultRequestHeaders.Add("Accept", "*/*");//"application/json");
                //c.DefaultRequestHeaders.Add("User-Agent", "Vehicle tracker");
                c.Timeout = new TimeSpan(0, 0, 50);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseBrowserLink();
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
                endpoints.MapRazorPages();
            });
        }
    }
}
