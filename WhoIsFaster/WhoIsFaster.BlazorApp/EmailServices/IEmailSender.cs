using System.Threading.Tasks;

namespace WhoIsFaster.BlazorApp.EmailServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}