//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmailConfirm
{
    using System;
    using System.Collections.Generic;
    
    public partial class BuyerRequest
    {
        public int Id { get; set; }
        public Nullable<int> SellerId { get; set; }
        public Nullable<int> BuyerId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string SellType { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string PhoneNo { get; set; }
        public Nullable<System.DateTime> DownloadDateTime { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual SellNote SellNote { get; set; }
    }
}