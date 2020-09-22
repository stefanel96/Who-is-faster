using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WhoIsFaster.BlazorApp.Areas.Identity.Pages.Account.Manage {
    public partial class EmailModel : PageModel {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EmailModel (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {
            [Required]
            [EmailAddress]
            [Display (Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync (IdentityUser user) {
            var email = await _userManager.GetEmailAsync (user);
            Email = email;

            Input = new InputModel {
                NewEmail = email,
            };

        }


        public async Task<IActionResult> OnGetAsync () {
            var user = await _userManager.GetUserAsync (User);
            if (user == null) {
                return NotFound ($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync (user);
            return Page ();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync () {
            var user = await _userManager.GetUserAsync (User);
            if (user == null) {
                return NotFound ($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid) {
                await LoadAsync (user);
                return Page ();
            }

            var email = await _userManager.GetEmailAsync (user);
            if (Input.NewEmail != email) {
                var setEmailResult = await _userManager.SetEmailAsync (user, Input.NewEmail);

                if (!setEmailResult.Succeeded) {
                    StatusMessage = "Unexpected error when trying to set email.";
                    return RedirectToPage ();
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your email is updated.";
                return RedirectToPage ();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage ();
        }

    }
}