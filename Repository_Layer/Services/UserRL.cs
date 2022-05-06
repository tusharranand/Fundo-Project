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
    }
}
