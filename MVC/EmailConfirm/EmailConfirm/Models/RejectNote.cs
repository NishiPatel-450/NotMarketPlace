using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmailConfirm.Models
{
    public class RejectNote
    {
        public int id { get; set; }
        public string title { get; set; }
        public string categoey { get; set; }
        public string remark { get; set; }
        public DateTime dateadd { get; set; }
        public string rejectedby {get;set;}
        public int seller { get; set; }
    }
}