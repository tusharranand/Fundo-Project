using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common_Layer.Users
{
    public class UserPostModel
    {
        [RegularExpression(@"^[A-Z][a-z]{2,15}$", ErrorMessage="Invalid Format")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[A-Z][a-z]{2,15}$", ErrorMessage = "Invalid Format")]
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{1,25}@[A-Za-z0-9]{2,25}[.][a-z]{2,5}([.][a-z]{2,5})?$", ErrorMessage = "Invalid Format")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{8,15}$", ErrorMessage = "Invalid Format")]
        public string Password { get; set; }
    }
}
