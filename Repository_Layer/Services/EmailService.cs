using Repository_Layer.FundooContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Repository_Layer.Services
{
    public class EmailService
    {
        public static void SendMail(string email, string token, FundooDbContext fundoo)
        {
            using(SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("apitesting09@gmail.com", "testing&123");

                var user = fundoo.User.FirstOrDefault(x => x.Email==email);
                MailMessage messageObj = new MailMessage();
                messageObj.To.Add(email);
                messageObj.From = new MailAddress("apitesting09@gmail.com");
                messageObj.Subject = "Passord Reset Link";
                messageObj.IsBodyHtml = true;
                messageObj.Body = "<!DOCTYPE html><html>" +
                    $"<h1 style=\"color:#c9370a\">Hello {user.FirstName} {user.LastName}</h1><br>" +
                    "<h3 style=\"color:#f09628\">If you have forgotten your password, copy the" +
                    " token given below and use it to change your password.</h3>" +
                    "<h3 style=\"color:#f09628\">(The token will only be valid for the next " +
                    "one hour.)</h3><br><br>" +
                    "<h2><u>Token:</u></h2>" +
                    $"<h3 style=\"color:#3a2ebf\">{token}</h3><br><br>" +
                    "<h3 style=\"color:#f09628\">This email was sent by Fundoo Notes.</h3>";
                client.Send(messageObj);
            }
        }
    }
}
