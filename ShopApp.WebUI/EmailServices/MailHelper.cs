using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ShopApp.WebUI.EmailServices
{

    /*
     Startup.cs=>
        services.AddTransient<IEmailSender, EmailSender>();     
      
    public class EmailSender : IEmailSender
    {
        private const string SendGridKey = "WebApi Key";

        public Task SendEmailAsync(string email, string htmlMessage, string subject, bool isHtml = true)
        {
            return Execute(SendGridKey, subject, htmlMessage, email);
        }
    }

    */

    public class MailHelper
    {
        public static bool SendMail(string body, string to, string subject, bool isHtml = true)
        {
            return SendMail(body, new List<string> { to }, subject, isHtml);
        }

        public static bool SendMail(string body, List<string> to, string subject, bool isHtml = true)
        {
            bool result = false;

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("test.emredemir@gmail.com");

                to.ForEach(x =>
                {
                    message.To.Add(new MailAddress(x)); //altanemre@gmail.com, emredemir@hotmail.com, uras@urs.com
                });

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("test.emredemir@gmail.com", "aw:DcsDTJJDXzy7");
                    smtp.Send(message);
                    result = true;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Mail Hata: {0}", e);
            }

            return result;
        }
    }
}
