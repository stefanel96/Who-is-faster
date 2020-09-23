using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using WhoIsFaster.BlazorApp.EmailServices;

namespace WhoIsFaster.BlazorApp.Pages {

    public partial class Contact {
        
        [Inject]
        public IConfiguration Configuration { get; set; }
        [Inject]
        public IEmailSender EmailSender { get; set; }

        public ContactFormModel _ContactFormModel { get; set; } = new ContactFormModel();

        private bool displaySendAlert { get; set; } = false;

        protected async Task HandleValidSubmit()
        {
            string body = "<h2>Reach client:</h2><ul><li>Email: "+_ContactFormModel.Email+"</li><li>Phone number: "+_ContactFormModel.Phone+"</li></ul>"
                            +"<h2>Question/Message:</h2><p>"+_ContactFormModel.Message+"</p>";
            await EmailSender.SendEmailAsync(Configuration["EmailSettings:Sender"],"Support(from contact us): "+_ContactFormModel.Name+" has a question",body);
            displaySendAlert = true;
        }

        protected void HandleInvalidSubmit()
        {
            displaySendAlert = false;
        }


    }

    public class ContactFormModel {
        [Required]
        [MaxLength (100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }

    }

}