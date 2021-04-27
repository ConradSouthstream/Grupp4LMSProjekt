using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web
{ 
        public static class Roles
        {
            public const string ElevRole = "Elev";
            public const string LarareRole = "Larare";
        }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var WebHost = CreateHostBuilder(args).Build();
            using (var scope = WebHost.Services.CreateScope())
            {
                await AddAppRoles(scope.ServiceProvider);
            }

            await WebHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        private static async  Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager,string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

        }
        private static async Task AssingRoleToUser(UserManager<Anvandare> userManager,string userEmail ,string roleName)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (userEmail !=null)
            {
                await userManager.AddToRoleAsync(user,roleName);
            }
        }

        private static  async Task AddAppRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Anvandare>>();
            string elevRole = Roles.ElevRole;
            string larareRole = Roles.LarareRole;
            await CreateRoleIfNotExists(roleManager, larareRole).ConfigureAwait(false);
            //await CreateRoleIfNotExists(roleManager, elevRole).ConfigureAwait(false);

            var elevEmail = "elev@test.com";
            var larareEmail = "larare@test.com";
            //await AssingRoleToUser(userManager, elevEmail, elevRole).ConfigureAwait(false);
            await AssingRoleToUser(userManager, larareEmail, larareRole).ConfigureAwait(false);

        }



    }
}
