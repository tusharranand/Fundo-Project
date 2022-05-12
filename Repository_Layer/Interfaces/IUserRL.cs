using Common_Layer;
using Common_Layer.Users;
using Repository_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interfaces
{
    public interface IUserRL
    {
        public void AddUser(UserPostModel user);
        public string LoginUser(string email, string password);
        public bool ForgotPassword(string email);
        public IEnumerable<User> GetAll();
        public bool ChangePassword(ChangePasswordModel newPassword, string Email);
    }
}
