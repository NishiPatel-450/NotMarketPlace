using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmailConfirm.Models
{
    public class Register
    {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailId { get; set; }

            [Required(ErrorMessage = "Required.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Required.")]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }
            public string scheme { get; set; }
            public string host { get; set; }
            public string port { get; set; }
    }
}