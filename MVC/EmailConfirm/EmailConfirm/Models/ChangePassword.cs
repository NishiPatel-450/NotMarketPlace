using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmailConfirm.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}