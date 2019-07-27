#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Net;
#endregion

namespace FriendCash.Service.Configs
{
   public class Email
   {

      #region SmtpHost
      private static string SmtpHost
      {
         get { return ConfigurationManager.AppSettings["emailService:SmtpHost"]; }
      }
      #endregion

      #region SmtpPort
      private static int SmtpPort
      {
         get { return int.Parse(ConfigurationManager.AppSettings["emailService:SmtpPort"]); }
      }
      #endregion

      #region FromAddress
      private static string FromAddress
      {
         get { return ConfigurationManager.AppSettings["emailService:FromAddress"]; }
      }
      #endregion

      #region FromPassword
      private static string FromPassword
      {
         get { return ConfigurationManager.AppSettings["emailService:FromPassword"]; }
      }
      #endregion

      #region FromName
      private static string FromName
      {
         get { return ConfigurationManager.AppSettings["emailService:FromName"]; }
      }
      #endregion

      #region SendAsync
      public static async Task SendAsync(string Subject, string Body, string DestinationAddress)
      {
         try
         {

            // MESSAGE            
            var myMessage = new MailMessage();
            myMessage.Subject = Subject;
            myMessage.Body = Body;
            myMessage.IsBodyHtml = true;
            myMessage.To.Add(DestinationAddress);
            myMessage.From = new MailAddress(Email.FromAddress, Email.FromName);

            // SMTP
            var mySmtp = new SmtpClient(Email.SmtpHost, Email.SmtpPort);
            mySmtp.Credentials = new NetworkCredential(Email.FromAddress, Email.FromPassword);

            // SEND 
            await mySmtp.SendMailAsync(myMessage);

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}