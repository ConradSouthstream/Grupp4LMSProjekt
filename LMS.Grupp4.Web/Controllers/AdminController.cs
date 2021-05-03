using LMS.Grupp4.Core.Dtos;
using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class AdminController : Controller
    {
        private readonly UserManager<Anvandare> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IPasswordHasher<Anvandare> passwordHasher;

        public AdminController(UserManager<Anvandare> userManager, RoleManager<IdentityRole> roleManager,IPasswordHasher<Anvandare> passwordHasher)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.passwordHasher = passwordHasher;
        }
      
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.allRoles = new SelectList(await roleManager.Roles.ToListAsync(), "Id", "Name");
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnvandareDto user)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(user.RoleId);

                var anvandare = new Anvandare
                {
                    EfterNamn = user.EfterNamn,
                    ForNamn = user.ForNamn,
                    UserName = user.Email,
                    Email = user.Email,
                    Avatar = user.Avatar
                };

                var result = await userManager.CreateAsync(anvandare, user.Password);
                if (result.Succeeded)
                {
                    // user created: add it to role
                    var res = await userManager.AddToRoleAsync(anvandare, role.Name);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors) 
                        ModelState.AddModelError("", error.Description);
                    
                }
            }
            ViewBag.allRoles = new SelectList(await roleManager.Roles.ToListAsync(), "Id", "Name");
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
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

