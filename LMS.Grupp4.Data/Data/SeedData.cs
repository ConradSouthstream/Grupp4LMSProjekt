using Bogus;
using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(IServiceProvider services)
        {
            using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                 // if (await context.Kurser.AnyAsync()) return;

                var userManager = services.GetRequiredService<UserManager<Anvandare>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var fake = new Faker("sv");

                var kurser = new List<Kurs>();

                for (int i = 0; i < 20; i++)
                {
                    var kurs = new Kurs
                    {
                        Namn= fake.Company.CatchPhrase(),
                        Beskrivning= fake.Hacker.Verb(),
                       // Duration = new TimeSpan(0, 55, 0),
                        StartDatum = DateTime.Now.AddDays(fake.Random.Int(-2, 2))
                    };

                    kurser.Add(kurs);
                }

                await context.AddRangeAsync(kurser);

                var roleNames = new[] { "Elev", "Larare" };

                foreach (var roleName in roleNames)
                {
                    if (await roleManager.RoleExistsAsync(roleName)) continue;

                    var role = new IdentityRole { Name = roleName };
                    var result = await roleManager.CreateAsync(role);

                    if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                }

                var larareEmail = "larare@lms.se";

                var foundLarare = await userManager.FindByEmailAsync(larareEmail);

                if (foundLarare != null) return;

                var admin = new Anvandare
                {
                    UserName = "Larare",
                    Email = larareEmail,
                };

                var addLarareResult = await userManager.CreateAsync(admin);

                if (!addLarareResult.Succeeded) throw new Exception(string.Join("\n", addLarareResult.Errors));

                var larareUser = await userManager.FindByEmailAsync(larareEmail);

                foreach (var role in roleNames)
                {
                    if (await userManager.IsInRoleAsync(larareUser, role)) continue;

                    var addToRoleResult = await userManager.AddToRoleAsync(larareUser, role);

                    if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }

                await context.SaveChangesAsync();

            }
        }



    }
}
