using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Core.ViewModels.Admin;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db; // remove later
        private readonly UserManager<Anvandare> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        //private readonly IPasswordHasher<Anvandare> passwordHasher;
        private readonly IUnitOfWork uow;
        private readonly SignInManager<Anvandare> signInManager;

        public AdminController(ApplicationDbContext context, IUnitOfWork unitOfWork,
                               SignInManager<Anvandare> signInManager,
                               UserManager<Anvandare> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IConfiguration config
                               )
        {
            this.uow = unitOfWork;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
            //this.passwordHasher = passwordHasher;
            db = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var larare = new List<Anvandare>();
            var elever = new List<Anvandare>();
            foreach (var user in userManager.Users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var roleName = userRoles.FirstOrDefault();
                if (roleName == "Lärare")
                {
                    user.IsLarare = true;
                    larare.Add(user);
                }
                else
                {
                    user.IsLarare = false;
                    elever.Add(user);
                }
            }
            var model = new AdminListUsersViewModel
            {
                Larare = larare,
                Elever = elever
            };
            return View(model);
        }


        public async Task<IActionResult> CreateLarare()
        {
            var kurser = await uow.KursRepository.GetAllKurserAsync();
            var model = new AdminCreateLarareViewModel
            {
                Kurser = kurser
            };
            return View("CreateLarare", model);
        }

        public async Task<IActionResult> CreateElev()
        {
            var kurser = await uow.KursRepository.GetAllKurserAsync();
            var model = new AdminCreateElevViewModel
            {
                Kurser = kurser
            };
            return View("CreateElev", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatLarare([FromBody] AdminCreateLarareViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Anvandare
                {
                    EfterNamn = model.EfterNamn,
                    ForNamn = model.ForNamn,
                    UserName = model.Email,
                    Email = model.Email,
                    Avatar = model.Avatar
                };

                var result = await userManager.CreateAsync(user, config["AdminPw"]);
                if (result.Succeeded)
                {
                    // user created: add it to role
                    var role = await roleManager.FindByIdAsync("Lärare");
                    var res = await userManager.AddToRoleAsync(user, role.Name);
                    // add user to kurs
                    foreach (var kursId in model.KursId)
                    {
                        var kurs = await uow.KursRepository.GetKursAsync(kursId);
                        db.AnvandareKurser.Add(new AnvandareKurs
                        {
                            Anvandare = user,
                            Kurs = kurs
                        });
                    }
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


        [HttpPost]
        public async Task<IActionResult> CreateElev(AdminCreateElevViewModel model)
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
                    Avatar = model.Avatar
                };

                var result = await userManager.CreateAsync(user, config["AdminPw"]);
                if (result.Succeeded)
                {
                    // user created: add it to role
                    var res = await userManager.AddToRoleAsync(user, "Elev");
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

        public async Task<IActionResult> UpdateElev(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                var kurs = db.AnvandareKurser.Where(k => k.AnvandareId == user.Id).FirstOrDefault();
                var allaKurser = await uow.KursRepository.GetAllKurserAsync();
                var model = new AdminCreateElevViewModel
                {
                    ForNamn = user.ForNamn,
                    EfterNamn = user.EfterNamn,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    //KursId = kurs.Id,
                    //Kurser = allaKurser
                };
               
                return View(model);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateElev(string id, AdminCreateElevViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);
            if (model != null)
            {
                var kurs = await uow.KursRepository.GetKursAsync(model.KursId);
                var kursList = new List<Kurs>();
                kursList.Add(kurs);
                if (!string.IsNullOrEmpty(model.Email))
                    user.Email = model.Email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.ForNamn = model.ForNamn;
                    user.EfterNamn = model.EfterNamn;
                    user.Email = model.Email;
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

        public async Task<IActionResult> UpdateLarare(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                var kurs = db.AnvandareKurser.Where(k => k.AnvandareId == user.Id).FirstOrDefault();
                var allaKurser = await uow.KursRepository.GetAllKurserAsync();
                var model = new AdminCreateElevViewModel
                {
                    ForNamn = user.ForNamn,
                    EfterNamn = user.EfterNamn,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    //KursId = kurs.Id,
                    //Kurser = allaKurser
                };

                return View(model);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLarare(string id, AdminCreateElevViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);
            if (model != null)
            {
                var kurs = await uow.KursRepository.GetKursAsync(model.KursId);
                var kursList = new List<Kurs>();
                kursList.Add(kurs);
                if (!string.IsNullOrEmpty(model.Email))
                    user.Email = model.Email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.ForNamn = model.ForNamn;
                    user.EfterNamn = model.EfterNamn;
                    user.Email = model.Email;
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

