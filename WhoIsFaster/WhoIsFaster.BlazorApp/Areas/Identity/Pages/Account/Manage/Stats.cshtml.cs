using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;


namespace WhoIsFaster.BlazorApp.Areas.Identity.Pages.Account.Manage
{
    public partial class StatsModel : PageModel
    {
        private readonly IRegularUserService _regularUserService;
        private readonly UserManager<IdentityUser> _userManager;

        public StatsModel(IRegularUserService regularUserService, UserManager<IdentityUser> userManager)
        {
            _regularUserService = regularUserService;
            _userManager = userManager;
        }


        public RegularUserDTO RegularUser { get; set; }


        private async Task LoadAsync(IdentityUser user)
        {

            var username = await _userManager.GetUserNameAsync(user);
            RegularUser = await _regularUserService.GetRegularUserByUserNameAsync(username);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

    }
}