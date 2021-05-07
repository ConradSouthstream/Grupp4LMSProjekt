using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LMS.Grupp4.Core.Entities;

namespace LMS.Grupp4.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<Anvandare> _userManager;
        private readonly SignInManager<Anvandare> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Anvandare> signInManager,
                          ILogger<LoginModel> logger,
                          UserManager<Anvandare> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                    var roles = await _signInManager.UserManager.GetRolesAsync(user);
                    // the user should have only one role "Elev" or "Lärare"
                    var role = roles.FirstOrDefault();
                    if (role == "Elev")
                    {
                        return Redirect("~/Elev");
                    }
                    else if (role == "Lärare")
                    {
                        return Redirect("~/Home/Larare");
                    }
                    _logger.LogInformation("User logged in but no role assigned.");
                    ModelState.AddModelError(string.Empty, "No role assiged for this user.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

