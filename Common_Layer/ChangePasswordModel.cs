using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common_Layer
{
    public class ChangePasswordModel
    {
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{8,15}$", ErrorMessage = "Invalid Format")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{8,15}$", ErrorMessage = "Invalid Format")]
        public string ConfirmPassword { get; set; }
    }
}
