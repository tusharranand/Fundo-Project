using Common_Layer.Users;
using Microsoft.AspNetCore.Mvc;
using Repository_Layer.FundooContext;
using Repository_Layer.Interfaces;
using System;

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
        [HttpPost("LoginUser")]
        public ActionResult LoginUser(string email, string password)
        {
            try
            {
                string token = this.userBL.LoginUser(email, password);
                if (token == null)
                    return this.BadRequest(new { success = false, message = $"Email or Password is Invalid" });
                return this.Ok(new { success = true, message = $"Token Generated is: {token}" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = $"Login Failed {e.Message}" });
            }
        }
        [HttpPost("ForgotPassword")]
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
    }
}
