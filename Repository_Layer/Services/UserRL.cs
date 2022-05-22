using Common_Layer;
using Common_Layer.Users;
using Experimental.System.Messaging;
using Microsoft.EntityFrameworkCore;
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
        Encryption encryptDecrypt;
        public UserRL(FundooDbContext fundoo, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.fundoo = fundoo;
            this.encryptDecrypt = new Encryption();
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
                userData.Password = encryptDecrypt.EncryptString(user.Password);

                fundoo.Add(userData);
                fundoo.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Encrypt/decrypt https://www.c-sharpcorner.com/UploadFile/2b481f/encrypt-and-decrypt-text-in-web-api/

        public string LoginUser(string email, string password)
        {
            try
            {
                var user = fundoo.User.FirstOrDefault(u => u.Email == email);
                if (user == null)
                    return null;
                string decryptedPass = encryptDecrypt.DecryptString(user.Password);
                if (decryptedPass == password)
                    return GenerateJWTToken(email, user.UserID);
                throw new Exception("Incorrect Password");
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GenerateJWTToken(string Email, int UserID)
        {
            //generate token

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email", Email),
                    new Claim("UserID", UserID.ToString())
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
                message.Formatter = new BinaryMessageFormatter();;
                message.Body = GenerateJWTToken(email, user.UserID);
                EmailService.SendMail(email, message.Body.ToString(), fundoo);
                FundooQ.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);
                FundooQ.BeginReceive();
                FundooQ.Close();
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
                EmailService.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()), fundoo);
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
        public IEnumerable<User> GetAll()
        {
            try
            {
                foreach (var user in fundoo.User)
                    user.Password = encryptDecrypt.DecryptString(user.Password);
                return fundoo.User;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangePassword(ChangePasswordModel newPassword, string Email)
        {
            try
            {
                var user = fundoo.User.FirstOrDefault(x => x.Email == Email);
                if (newPassword.Password.Equals(newPassword.ConfirmPassword))
                {
                    var pass = encryptDecrypt.EncryptString(newPassword.Password);
                    user.Password = pass.ToString();
                    fundoo.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
