using Common_Layer.Users;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository_Layer.Entities;
using Repository_Layer.FundooContext;
using Repository_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Repository_Layer.Services
{
    public class UserRL : IUserRL
    {
        FundooDbContext fundoo;
        public IConfiguration Configuration { get; }
        public UserRL(FundooDbContext fundoo, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.fundoo = fundoo;
        }
        public void AddUser(UserPostModel user)
        {
            try
            {
                User userData = new User();
                userData.FirstName = user.FirstName;
                userData.LastName = user.LastName;
                userData.RegisterDate = DateTime.Now;
                userData.Email = user.Email;
                userData.Password = user.Password;

                fundoo.Add(userData);
                fundoo.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string LoginUser(string email, string password)
        {
            try
            {
                var user = fundoo.User.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (user == null)
                    return null;
                return GenerateJWTToken(email, user.UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GenerateJWTToken(string email, int userID)
        {
            //generate token

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("userID", userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ForgotPassword(string email)
        {
            try
            {
                var user = fundoo.User.FirstOrDefault(u => u.Email == email);
                if (user == null)
                    return false;
                MessageQueue FundooQ;
                //Add message to queue
                if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                    FundooQ = new MessageQueue(@".\Private$\FundooQueue");
                else FundooQ = MessageQueue.Create(@".\Private$\FundooQueue");

                Message message= new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = GenerateJWTToken(email, user.UserID);
                EmailService.SendMail(email, message.Body.ToString());
                FundooQ.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailService.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                    MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                };
            }
        }

        private string GenerateToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
