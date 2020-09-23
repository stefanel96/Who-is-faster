using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using WhoIsFaster.ApplicationServices.Exceptions;

namespace WhoIsFaster.BlazorApp.EmailServices
{
    public class EmailSender : IEmailSender
    {   
        private readonly EmailSettings _emailSettings;
        private readonly IHostEnvironment _configuration;

        public EmailSender(IOptions<EmailSettings> emailSettings,IHostEnvironment configuration)
        {
            _configuration = configuration;
            _emailSettings = emailSettings.Value;
        }

        [Obsolete]
        public async Task SendEmailAsync(string email, string subject, string message)
        {
               try
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

            mimeMessage.To.Add(new MailboxAddress(email));

            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart("html")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                if (_configuration.IsDevelopment())
                {
                    // The third parameter is useSSL (true if the client should make an SSL-wrapped
                    // connection to the server; otherwise, false).
                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);
                }
                else
                {
                    await client.ConnectAsync(_emailSettings.MailServer);
                }

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                await client.SendAsync(mimeMessage);

                await client.DisconnectAsync(true);
            }

        }
        catch (Exception ex)
        {
            throw new WhoIsFasterException(ex.Message);
        }
        }
    }
}