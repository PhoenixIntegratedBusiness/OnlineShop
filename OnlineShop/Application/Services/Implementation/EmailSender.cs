using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class EmailSender : IEmailSender
    {

        private readonly ILogger<EmailSender> _logger;
         
        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("info.farnaz.zm@gmail.com", "Password Reset Verification Code");
                mail.To.Add(to); //its list we can add n emial
                mail.Subject = subject;
                mail.Body = body;   
                mail.IsBodyHtml = true;   //ایا توی بادی تگ اچ تی ام ال هست

                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;

                SmtpServer.Credentials = new System.Net.NetworkCredential("info.farnaz.zm@gmail.com", "x s d u x z j a d l n m s k v n");
                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception exception)     
            {
                _logger.LogError($"Email Error\n\tErrorMessage:: {exception.Message}");
                return false;
            }
        }

    }
}
