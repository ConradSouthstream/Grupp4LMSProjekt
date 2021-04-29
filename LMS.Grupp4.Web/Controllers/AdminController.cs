using LMS.Grupp4.Core.Dtos;
using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{

    public class AdminController : Controller
    {
        private readonly UserManager<Anvandare> userManager;

        public AdminController(UserManager<Anvandare> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnvandareDto user)
        {
            if (ModelState.IsValid)
            {
                var anvandare = new Anvandare
                {
                    UserName = user.Name,
                    Email = user.Email
                };

                var result = await userManager.CreateAsync(anvandare, user.Password);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
    }
}

