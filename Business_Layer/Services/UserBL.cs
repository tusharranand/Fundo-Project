using Common_Layer;
using Common_Layer.Users;
using Repository_Layer.Entities;
using Repository_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Layer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public void AddUser(UserPostModel user)
        {
            try
            {
                this.userRL.AddUser(user);
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
                return this.userRL.LoginUser(email, password);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ForgotPassword(string email)
        {
            try
            {
                return this.userRL.ForgotPassword(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return this.userRL.GetAll();
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
                return this.userRL.ChangePassword(newPassword, Email);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
