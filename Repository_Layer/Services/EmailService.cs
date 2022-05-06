using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Repository_Layer.Services
{
    public class EmailService
    {
        public static void SendMail(string email, string token)
        {
            using(SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("apitesting09@gmail.com", "testing&123");

                MailMessage messageObj = new MailMessage();
                messageObj.To.Add(email);
                messageObj.From = new MailAddress("apitesting09@gmail.com");
                messageObj.Subject = "Passord Reset Link";
                messageObj.Body = $"www.fundooNotes.com/reset-password/{token}";
                client.Send(messageObj);
            }
        }
    }
}
