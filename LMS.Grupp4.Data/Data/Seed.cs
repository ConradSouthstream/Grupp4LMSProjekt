using Bogus;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Data
{
    public static class Seed
    {
        private static Faker fake;

        public static async Task InitAsync(IServiceProvider services,string adminPw)
        {
            using (var db = services.GetRequiredService<ApplicationDbContext>())
            {
                fake = new Faker("sv");
                var userManager = services.GetRequiredService<UserManager<Anvandare>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                if (db.Anvandare.Any())
                {
                    return;
                }
                db.Database.EnsureDeleted();
                db.Database.Migrate();

                var dokumentsTyper = new List<DokumentTyp>()
                {                     
                    new DokumentTyp(){Namn = "Generalla Information"},
                    new DokumentTyp(){Namn = "Modul Information"},
                    new DokumentTyp(){Namn = "Inlämning"},
                    new DokumentTyp(){Namn = "Föreläsning"},
                    new DokumentTyp(){Namn = "Uppgift"},
                    new DokumentTyp(){Namn = "Föreläsning"},
                    new DokumentTyp(){Namn = "E-learning"},
                };
                await db.AddRangeAsync(dokumentsTyper);

                var aktivitetTyper = new List<AktivitetTyp>()
                {
                    new AktivitetTyp(){Namn = "Föreläsning"},
                    new AktivitetTyp(){Namn = "E-learning"},
                    new AktivitetTyp(){Namn = "Uppgift"},
                };
                
                await db.AddRangeAsync(aktivitetTyper);

                var roleNames = new[] { "Elev", "Lärare" };

                foreach (var roleName in roleNames)
                {
                    if (await roleManager.RoleExistsAsync(roleName)) continue;

                    var role = new IdentityRole { Name = roleName };
                    var result = await roleManager.CreateAsync(role);

                    if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                }

                //Skapar Kurser, Moduler och Aktiviteter
                var kurser = new List<Kurs>();
                var moduler = new List<Modul>();
                var aktiviteter = new List<Aktivitet>();
                var elever = new List<Anvandare>();
                var enrollments = new List<AnvandareKurs>();
               // var aktuellTid = DateTime.Now;

                for (int i = 0; i < 10; i++)
                {
                    //första kursen skapas 10 veckor tidigare från dagens datum
                    DateTime startdatum = DateTime.Now.Date.AddDays(((i + 1) * 35)-105);
                    var kurs = new Kurs
                    {
                        Namn = $"Kurs {i + 1}",
                        Beskrivning = fake.Lorem.Paragraph(),
                        StartDatum = startdatum,
                        SlutDatum = startdatum.AddDays(34),
                    };
                    if (kurs.SlutDatum < DateTime.Now && kurs.StartDatum< DateTime.Now)
                    {
                    kurs.KursStatus = Status.Avslutad;
                    }
                    if (kurs.StartDatum <= DateTime.Now && kurs.SlutDatum.AddHours(23) >= DateTime.Now)
                    {
                        kurs.KursStatus = Status.Aktuell;
                    }
                    if (kurs.StartDatum > DateTime.Now.AddDays(1)&&kurs.SlutDatum> DateTime.Now)
                    {
                        kurs.KursStatus = Status.Kommande;
                    }
                    //Tillägg 3 elever per kurs
                    for (int Useri = 0; Useri < 3; Useri++)
                    {
                        var fName = fake.Name.FirstName();
                        var lName = fake.Name.LastName();

                        var user = new Anvandare
                        {
                            ForNamn = fName,
                            EfterNamn = lName,
                            Email = fake.Internet.Email(fName,lName),
                            Avatar = fake.Internet.Avatar(),

                        };
                        user.UserName = user.Email;
                        await userManager.FindByEmailAsync(user.Email);
                        await userManager.CreateAsync(user, adminPw);
                        await userManager.AddToRoleAsync(user, "Elev");
                        elever.Add(user);
                        var enrollment = new AnvandareKurs
                        {
                            Kurs = kurs,
                            Anvandare = user,
                            Betyg = fake.Random.Int(1, 5)
                        };
                       enrollments.Add(enrollment);
                    }
                    //Tillägg av 1 lärare per Kurs
                    for (int Lärarei = 0; Lärarei < 1; Lärarei++)
                    {
                        var lärarefName = fake.Name.FirstName();
                        var lärarelName = fake.Name.LastName();

                        var lärare = new Anvandare
                        {
                            ForNamn = lärarefName,
                            EfterNamn = lärarelName,
                            Email = fake.Internet.Email(lärarefName, lärarelName),
                            Avatar = fake.Internet.Avatar(),

                        };
                        lärare.UserName = lärare.Email;
                        await userManager.FindByEmailAsync(lärare.Email);
                        await userManager.CreateAsync(lärare, adminPw);
                        await userManager.AddToRoleAsync(lärare, "Lärare");
                        elever.Add(lärare);
                        var lärareEnrollment = new AnvandareKurs
                        {
                            Kurs = kurs,
                            Anvandare = lärare,
                            Betyg = fake.Random.Int(1, 5)
                        };
                        enrollments.Add(lärareEnrollment);
                    }
                    //varje kurs tillsätter 5 moduler
                    for (int moduli = 0; moduli < 5; moduli++)
                    {
                        var modulstartdatum = startdatum.AddDays(((moduli + 1) * 7) - 7);
                        var modul = new Modul
                        {
                            Namn = $"Modul {i + 1}.{moduli + 1}",
                            Beskrivning = fake.Lorem.Paragraph(),
                            Kurs = kurs,
                            StartDatum = modulstartdatum,
                            SlutDatum = modulstartdatum.AddDays(6)
                        };
                        //varje modul tillsätter 2 aktiviteter
                        for (int aktiviteti = 0; aktiviteti < 2; aktiviteti++)
                        {
                            var aktivitet = new Aktivitet
                            {
                                Namn = $"Aktivitet {i+1}.{moduli+1}.{aktiviteti+1}",
                                Beskrivning = fake.Lorem.Paragraph(),
                                AktivitetTyp = fake.Random.ListItem<AktivitetTyp>(aktivitetTyper),
                                Modul = modul,
                                StartDatum = modulstartdatum.AddDays(((aktiviteti + 1) * 3) - 3),
                                SlutDatum = modulstartdatum.AddDays(((aktiviteti + 1) * 3) -1)
                            };
                            aktiviteter.Add(aktivitet);
                        }
                        moduler.Add(modul);
                    }
                    kurser.Add(kurs);
                }


                await db.AddRangeAsync(enrollments);
                await db.AddRangeAsync(kurser);
                                
                await db.AddRangeAsync(moduler);
                
                await db.AddRangeAsync(aktiviteter);

                //Skapar statiska elev och lärar konto
                var larareEmail = "larare@lms.se";
                var elevEmail = "elev@lms.se";
                var foundLarare = await userManager.FindByEmailAsync(larareEmail);
                var foundElev = await userManager.FindByEmailAsync(elevEmail);

                if (foundLarare != null) return;
                if (foundElev != null) return;

                var admin = new Anvandare
                {
                    ForNamn = "Admin",
                    EfterNamn = "Lärare",
                    Avatar = fake.Internet.Avatar(),
                    UserName = larareEmail,
                    Email = larareEmail,
                };
                var elev = new Anvandare
                {
                    ForNamn="Elev",
                    EfterNamn="en",
                    Avatar = fake.Internet.Avatar(),
                    UserName = elevEmail,
                    Email = elevEmail,
                };
                var addLarareResult = await userManager.CreateAsync(admin, adminPw);
                var addelevResult = await userManager.CreateAsync(elev, adminPw);
                if (!addLarareResult.Succeeded) throw new Exception(string.Join("\n", addLarareResult.Errors));
                if (!addelevResult.Succeeded) throw new Exception(string.Join("\n", addelevResult.Errors));
                var larareUser = await userManager.FindByEmailAsync(larareEmail);
                var elevUser = await userManager.FindByEmailAsync(elevEmail);
                if (await userManager.IsInRoleAsync(larareUser, "Lärare")) return;
                if (await userManager.IsInRoleAsync(elevUser, "Elev")) return;
                await userManager.AddToRoleAsync(larareUser, "Lärare");
                await userManager.AddToRoleAsync(elevUser, "Elev");
                var statiskElevEnrollment = new AnvandareKurs
                {
                    Kurs = kurser.First(),
                    Anvandare = elev,
                    Betyg = fake.Random.Int(1, 5)
                };
                var statiskLärareEnrollment = new AnvandareKurs
                {
                    Kurs = kurser.First(),
                    Anvandare = admin,
                    Betyg = 5
                };
                await db.AddRangeAsync(statiskLärareEnrollment);
                await db.AddRangeAsync(statiskElevEnrollment);
                await db.SaveChangesAsync();
            }

        }
    }
}

