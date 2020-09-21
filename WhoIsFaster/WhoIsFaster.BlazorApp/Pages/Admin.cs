using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.BlazorApp.ViewModels;

namespace WhoIsFaster.BlazorApp.Pages
{
    public partial class Admin
    {
        [Inject]
        public ITextService textService { get; set; }
        public string Title { get; set; } = "Admin";

        private IEnumerable<TextVM> texts;

        public Text _Text { get; set; } = new Text();
        private bool saved;

        private async Task ValidSubmit()
        {
            await textService.CreateTextAsync(_Text.Source, _Text.TextContent);
            saved = true;
            _Text.Source = string.Empty;
            _Text.TextContent = string.Empty;
            OnInitializedAsync();

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
            StateHasChanged();
            texts = TextVMExtensions.ToTextVMs(await textService.GetAllTextsAsync());
            StateHasChanged();

        }

        private async void DeleteText(int id)
        {
            await textService.DeleteTextAsync(id);
            OnInitializedAsync();
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
