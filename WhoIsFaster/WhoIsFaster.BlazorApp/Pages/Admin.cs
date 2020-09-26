using Microsoft.AspNetCore.Components;
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
        public string Title { get; set; } = "Admin Panel";
        private IEnumerable<TextVM> texts;
        private IEnumerable<TextVM> hiddenTexts;

        public Text _Text { get; set; } = new Text();
        public bool Saved { get; set; }

        private async Task ValidSubmit()
        {
            await textService.CreateTextAsync(_Text.Source, _Text.TextContent);
            Saved = true;
            _Text.Source = string.Empty;
            _Text.TextContent = string.Empty;
            await OnInitializedAsync();
        }

        private void InvalidSubmit()
        {
            Saved = false;
        }

        private void HideAlert()
        {
            Saved = false;
        }
        protected override async Task OnInitializedAsync()
        {   
                Saved = false;
                StateHasChanged();
                texts = TextVMExtensions.ToTextVMs(await textService.GetAllTextsAsync());
                hiddenTexts = TextVMExtensions.ToTextVMs(await textService.GetAllHiddenTextsAsync());
                StateHasChanged();
        }

        private async void DeleteText(int id)
        {   
            Saved = false;
            await textService.DeleteTextAsync(id);
            await OnInitializedAsync();
        }

        private async void RecoverText(int id)
        {
            Saved = false;
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
