using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.BlazorApp.ViewModels;

namespace WhoIsFaster.BlazorApp.Pages
{
    public partial class Admin
    {
        [Inject]
        public ITextService textService { get; set; }
        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public string Title { get; set; } = "Admin Panel";
        private IEnumerable<TextVM> texts;
        private IEnumerable<TextVM> hiddenTexts;

        public Text _Text { get; set; } = new Text();
        private bool saved;

        private async Task ValidSubmit()
        {
            await textService.CreateTextAsync(_Text.Source, _Text.TextContent);
            saved = true;
            _Text.Source = string.Empty;
            _Text.TextContent = string.Empty;
            await OnInitializedAsync();
        }

        private void InvalidSubmit()
        {
            saved = false;
        }

        private void HideAlert()
        {
            saved = false;
        }
        protected override async Task OnInitializedAsync()
        {   
            // Console.WriteLine("nesto");
            // Console.WriteLine(HttpContextAccessor.HttpContext.User.IsInRole("Admin"));
            // if(!HttpContextAccessor.HttpContext.User.IsInRole("Admin")){
                // Console.WriteLine("usao");
                // NavigationManager.NavigateTo("~/");
            // }else{
                saved = false;
                StateHasChanged();
                texts = TextVMExtensions.ToTextVMs(await textService.GetAllTextsAsync());
                hiddenTexts = TextVMExtensions.ToTextVMs(await textService.GetAllHiddenTextsAsync());
                StateHasChanged();
            // }
        }

        private async void DeleteText(int id)
        {   
            saved = false;
            await textService.DeleteTextAsync(id);
            await OnInitializedAsync();
        }

        private async void RecoverText(int id)
        {
            saved = false;
            await textService.RecoverTextAsync(id);
            await OnInitializedAsync();
        }


    }

    public class Text
    {
        [Required]
        [StringLength(100, ErrorMessage = "You have reached maximum number of charachters.")]
        public string Source { get; set; }
        [Required]
        public string TextContent { get; set; }

    }
}
