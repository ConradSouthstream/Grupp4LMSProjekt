using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Extensions
{
    public static class Roles
    {
        public const string ElevRole = "Elev";
        public const string LarareRole = "Larare";
    }
    static class RoleExtensions
    {

        public static async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

        }
        private static async Task AssingRoleToUser(UserManager<Anvandare> userManager, string userEmail, string roleName)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (userEmail != null)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }

        private static async Task AddAppRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Anvandare>>();
            string elevRole = Roles.ElevRole;
            string larareRole = Roles.LarareRole;
            await CreateRoleIfNotExists(roleManager, larareRole).ConfigureAwait(false);
            await CreateRoleIfNotExists(roleManager, elevRole).ConfigureAwait(false);

            var elevEmail = "elev@test.com";
            var larareEmail = "larare@test.com";
            await AssingRoleToUser(userManager, elevEmail, elevRole).ConfigureAwait(false);
            await AssingRoleToUser(userManager, larareEmail, larareRole).ConfigureAwait(false);

        }
    }

}
