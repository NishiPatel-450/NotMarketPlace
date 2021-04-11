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
    
    public partial class SellNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SellNote()
        {
            this.BuyerRequests = new HashSet<BuyerRequest>();
            this.Downloads = new HashSet<Download>();
            this.SellerNotesAttachements = new HashSet<SellerNotesAttachement>();
            this.SellerNotesReportedIssues = new HashSet<SellerNotesReportedIssue>();
            this.SellerNotesReviews = new HashSet<SellerNotesReview>();
            this.Tables = new HashSet<Table>();
            this.RejectedNotes = new HashSet<RejectedNote>();
        }
    
        public int Id { get; set; }
        public int Seller_Id { get; set; }
        public string title { get; set; }
        public string Category { get; set; }
        public byte[] Display_pic { get; set; }
        public byte[] Upload_note { get; set; }
        public string NoteType { get; set; }
        public Nullable<int> NumberofPages { get; set; }
        public string Description { get; set; }
        public string University_Name { get; set; }
        public string Country { get; set; }
        public string Course { get; set; }
        public string Course_Code { get; set; }
        public string Professor_name { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public string NotePrivew { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyerRequest> BuyerRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Download> Downloads { get; set; }
        public virtual Register Register { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNotesAttachement> SellerNotesAttachements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNotesReportedIssue> SellerNotesReportedIssues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNotesReview> SellerNotesReviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Table> Tables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RejectedNote> RejectedNotes { get; set; }
    }
}