using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmailConfirm.Models
{
    public class RegDetail
    {
        [Required(ErrorMessage = "Please Enter Email Addess")]
        public string EmailId { get; set; }
    }
}