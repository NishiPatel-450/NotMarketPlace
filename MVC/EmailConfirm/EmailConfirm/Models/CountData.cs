using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailConfirm.Models
{
    public class CountData
    {
        public int id { get; set; }
        public string title { get; set; }
        public string cate { get; set; }
        public string size { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int publisher { get; set; }
        public DateTime date { get; set; }
        public int no_dow { get; set; }
    }
}