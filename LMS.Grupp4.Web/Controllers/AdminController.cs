using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.Admin;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LMS.Grupp4.Web.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db; // remove later
        private readonly UserManager<Anvandare> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IPasswordHasher<Anvandare> passwordHasher;
        private readonly IConfiguration config;
        private readonly IUnitOfWork uow;
        private readonly SignInManager<Anvandare> signInManager;

        public AdminController(ApplicationDbContext context, IUnitOfWork unitOfWork,
                               SignInManager<Anvandare> signInManager,
                               UserManager<Anvandare> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IConfiguration config,
                               IPasswordHasher<Anvandare> passwordHasher)
        {
            this.uow = unitOfWork;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.passwordHasher = passwordHasher;
            db = context;
            this.config = config;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {

            return View(userManager.Users.Include(u => u.KurserAnvandare).ThenInclude(u => u.Kurs));
        }

        public async Task<IActionResult> Create()
        {
            // Skapa SelectList till Dropdown
            var roles = new SelectList(await roleManager.Roles.ToListAsync(), "Id", "Name");
            var kurser = new SelectList(await uow.KursRepository.GetAllKurserAsync(), "Id", "Namn");
            var model = new AdminCreateUserViewModel
            {
                Roles = roles,
                Kurser = kurser
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kurs = await uow.KursRepository.GetKursAsync(model.KursId);
                var user = new Anvandare
                {
                    EfterNamn = model.EfterNamn,
                    ForNamn = model.ForNamn,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.Telefonnummer,
                    Avatar = model.Avatar,
                };

                var result = await userManager.CreateAsync(user, config["AdminPw"]);
                if (result.Succeeded)
                {
                    // user created: add it to role
                    var role = await roleManager.FindByIdAsync(model.RoleId);
                    var res = await userManager.AddToRoleAsync(user, role.Name);
                    // add user to kurs

                    db.AnvandareKurser.Add(new AnvandareKurs
                    {
                        Anvandare = user,
                        Kurs = kurs
                    });
                    db.SaveChanges();
                    await uow.KursRepository.SaveAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CreateElev(string kursId)
        {
            // Hämta kurs med kursId
            var kurs = await uow.KursRepository.GetKursAsync(int.Parse(kursId));
            var model = new AdminCreateElevViewModel
            {
                KursId = kurs.Id,
                Kurs = kurs
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateElev(AdminCreateElevViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Anvandare
                {
                    EfterNamn = model.EfterNamn,
                    ForNamn = model.ForNamn,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.Telefonnummer,
                    Avatar = model.Avatar
                };

                var result = await userManager.CreateAsync(user, config["AdminPw"]);
                if (result.Succeeded)
                {
                    // Eleven skapad: lägg till den i bland avändare med rollen "Elev"
                    await userManager.AddToRoleAsync(user, "Elev");

                    // Lägg till användaren i kursen
                    db.AnvandareKurser.Add(new AnvandareKurs
                    {
                        AnvandareId = user.Id,
                        KursId = model.KursId
                    });
                    db.SaveChanges();
                    await uow.KursRepository.SaveAsync();
                    // Återvänd till kursens detaljsida
                    return RedirectToAction("Details", "Kurser", new { id = $"{model.KursId}" });
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            //var kurs = db.AnvandareKurser.Where(k => k.AnvandareId == user.Id).FirstOrDefault();
            var userRoles = await userManager.GetRolesAsync(user);
            var roleName = userRoles.FirstOrDefault();
            if (user != null && roleName != null)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                var allRoles = await roleManager.Roles.ToListAsync();
                var roles = new SelectList(allRoles, "Id", "Name");
                var allaKurser = await uow.KursRepository.GetAllKurserAsync();
                var kurser = new SelectList(allaKurser, "Id", "Namn");

                var model = new AdminCreateUserViewModel
                {
                    ForNamn = user.ForNamn,
                    EfterNamn = user.EfterNamn,
                    Email = user.Email,
                    Telefonnummer = user.PhoneNumber,
                    Avatar = user.Avatar,
                    RoleId = role.Id,
                    Roles = roles,
                    Kurser = kurser
                };

                return View(model);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, AdminCreateUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.Email))
                    user.Email = model.Email;
                else
                    ModelState.AddModelError("", "Email måste ha ett värde");

                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.ForNamn = model.ForNamn;
                    user.EfterNamn = model.EfterNamn;
                    user.Email = model.Email;
                    user.PhoneNumber = model.Telefonnummer;
                    user.Avatar = model.Avatar;
                 
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }


    }
}

