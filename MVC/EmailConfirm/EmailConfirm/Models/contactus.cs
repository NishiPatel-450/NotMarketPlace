using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class contactus
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public String EmailId { get; set; }
        [Required]
        public String subject { get; set; }
        [Required]
        public String Comments { get; set; }

    }
}