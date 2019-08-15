using System;
using System.Net.Mail;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.Helpers
{
   internal class Mail
   {

      private readonly IOptions<AppSettings> _appSettings;
      public Mail([FromServices] IOptions<AppSettings> appSettings) { this._appSettings = appSettings; }

      public async Task SendAsync(string subject, string body, params string[] addresses)
      {
         try
         {
            var mailSettings = this._appSettings.Value.Mail;
            if (mailSettings == null) { return; }

            // MESSAGE
            var mailMessage = new MailMessage();
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            // ADDRESSES
            mailMessage.From = new MailAddress(mailSettings.FromAddress, mailSettings.FromName);
            var toAddresses = addresses.Select(x => new MailAddress(x)).ToList();
            toAddresses.ForEach(x => mailMessage.To.Add(x));

            // SMTP
            var smtpServer = new SmtpClient(mailSettings.SmtpHost, mailSettings.SmtpPort);
            smtpServer.Credentials = new NetworkCredential(mailSettings.FromAddress, mailSettings.FromPassword);

            // SEND
            await smtpServer.SendMailAsync(mailMessage);

         }
         catch (Exception) { }
      }

   }
}
