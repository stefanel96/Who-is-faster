using WhoIsFaster.ApplicationServices.DTOs;

namespace WhoIsFaster.BlazorApp.ViewModels
{
    public class TextVM
    {
        public int Id { get; private set; }
        public string Source { get; private set; }
        public string TextContent { get; set; }
        public TextVM(TextDTO text)
        {
            Id = text.Id;
            Source = text.Source;
            TextContent = text.TextContent;
        }
    }
}
