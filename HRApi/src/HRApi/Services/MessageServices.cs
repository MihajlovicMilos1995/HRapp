using HRApi.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logon.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender()
        {
            Options = new AuthMessageSenderOptions
            {
                SendGridUser = "Evilsanta",
                SendGridKey = "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"
            };
        }
        //da se nadje
        //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

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            Execute(Options.SendGridKey, subject, message, email).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Joe@contoso.com", "Joe Smith"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
