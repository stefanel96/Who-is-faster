using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WhoIsFaster.BlazorApp.Pages {

    public partial class Contact {

        public ContactFormModel _ContactFormModel { get; set; } = new ContactFormModel();

        private bool displaySendAlert { get; set; } = false;

        protected void HandleValidSubmit()
        {
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