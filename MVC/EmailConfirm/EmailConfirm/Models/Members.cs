using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailConfirm.Models
{
    public class Members
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Joiningdate { get; set; }
        public int UnderReviewNotes { get; set; }
        public int PublishedNotes { get; set; }
        public int DownloadNotes { get; set; }
        public int TotalExpences { get; set; }
        public int TotalEarnings { get; set; }
        public DateTime dob { get; set; }
        public string phone { get; set; }
        public string college { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        
    }
}