using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using OutFitMaker.Domain.Constants.Statics;

namespace OutFitMaker.Domain.Helper
{
    public static class MailService
    {
        public static void SendMessage(string email, string subject, string body, string code = null)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("OutFitMaker",
            "minamamdoh150@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(email,
            email);
            message.To.Add(to);
            message.Subject = subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<div style='direction:ltr'; font-size : 18px;>Hi {email}<br/>{body}: <br/> <a style='font-size:24px;'>{code} </a></div>";
            message.Body = bodyBuilder.ToMessageBody();


            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(GlobalStatices.DefaultOurEmail, "xykb xhgd kdkl eqzh");
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
