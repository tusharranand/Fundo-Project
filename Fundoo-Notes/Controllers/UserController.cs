using Common_Layer;
using Common_Layer.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository_Layer.Entities;
using Repository_Layer.FundooContext;
using Repository_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Fundoo_Notes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        FundooDbContext fundoo;
        IUserBL userBL;
        public UserController(FundooDbContext fundoo, IUserBL userBL)
        {
            this.fundoo = fundoo;
            this.userBL = userBL;
        }
        [HttpPost("AddUser")]
        public ActionResult AddUser(UserPostModel user)
        {
            try
            {
                this.userBL.AddUser(user);
                return this.Ok(new { success = true, message = $"User Added Successfully" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = $"Registration Fail {e.Message}" });
            }        
        }
        [HttpPost("LoginUser/{Email}/{Password}")]
        public ActionResult LoginUser(string Email, string Password)
        {
            try
            {
                string token = this.userBL.LoginUser(Email, Password);
                if (token == null)
                    return this.BadRequest(new { success = false, message = $"Email or Password is Invalid" });
                return this.Ok(new { success = true, message = $"Token Generated is: {token}" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = $"Login Failed {e.Message}" });
            }
        }
        [HttpPost("ForgotPassword/{Email}")]
        public ActionResult ForgotPassword(string Email)
        {
            try
            {
                bool result = this.userBL.ForgotPassword(Email);
                if (result)
                    return this.Ok(new { success = true, message = $"Email sent to the given Email Address" });
                return this.BadRequest(new { success = false, message = $"Email does not exist" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = $"Password Reset Failed {e.Message}" });
            }
        }
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            try
            {
                return this.userBL.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordModel newPassword)
        {
            try
            {
                //var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserID").Value);
                var email = (currentUser.Claims.FirstOrDefault(c => c.Type == "Email").Value);
                bool res = userBL.ChangePassword(newPassword, email);
                if(res==true)
                {
                    return this.Ok(new { success = true, message = "Password Changed Successfully" });
                }
                return this.BadRequest(new { success = false, message = "Password was not Changed" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
