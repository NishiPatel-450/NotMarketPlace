using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmailConfirm.Models
{
    public class Register
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter First Name")]
        [Display(Name ="First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Enter Email Addess")]
        [Display(Name = "Email ID")]
        [StringLength(50)]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        [StringLength(50)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Display(Name = "Confirm Password")]
        [StringLength(50)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string scheme { get; set; }
        public string host { get; set; }
        public string port { get; set; }
    }
}