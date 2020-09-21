using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices;
using WhoIsFaster.ApplicationServices.Interfaces;

namespace WhoIsFaster.BlazorApp.Pages
{
    public partial class Admin
    {
        [Inject]
        public ITextService TextService { get; set; }
        public string Title { get; set; } = "Admin";

        public Text _Text { get; set; } = new Text();
        private bool saved;

        private async Task validSubmit()
        {
            saved = true;
            _Text.Source = string.Empty;
            _Text.TextContent = string.Empty;
        }

        private void invalidSubmit()
        {
            saved = false;
        }

        private void hideAlert()
        {
            saved = false;
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
