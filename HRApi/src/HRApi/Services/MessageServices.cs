﻿using HRApi.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
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
        // public AuthMessageSender()
        // {
        //     Options = new AuthMessageSenderOptions
        //     {
        //         SendGridUser = "Evilsanta",
        //         SendGridKey = "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"
        //     };
        // }

        //public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        //
        // public Task SendEmailAsync(string email, string subject, string message)
        // {
        //     // Plug in your email service here to send an email.
        //     Execute(Options.SendGridKey, subject, message, email).Wait();
        //     return Task.FromResult(0);
        // }
        //
        // public async Task Execute(string apiKey, string subject, string message, string email)
        // {
        //     var client = new SendGridClient(apiKey);
        //     var msg = new SendGridMessage()
        //     {
        //         From = new EmailAddress("Joe@contoso.com", "Joe Smith"),
        //         Subject = subject,
        //         PlainTextContent = message,
        //         HtmlContent = message
        //     };
        //     msg.AddTo(new EmailAddress(email));
        //     var response = await client.SendEmailAsync(msg);
        // }

        //open source service(Mailkit)
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("HrAdmin", "Support@HrApp.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };
            
            
                using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
                client.AuthenticationMechanisms.Remove("XOAUTH2"); // Must be removed for Gmail SMTP
                client.Authenticate("mihajlovicmilos16@gmail.com", "scarface995");
                client.Send(emailMessage);
                client.Disconnect(true);
                //client.LocalDomain = "smtp.gmail.com";
                
                //await client.ConnectAsync("smtp.relay.uri", 25, SecureSocketOptions.None).ConfigureAwait(false);
                //await client.SendAsync(emailMessage).ConfigureAwait(false);
                //await client.DisconnectAsync(true).ConfigureAwait(false);
            };
        }
    }
}