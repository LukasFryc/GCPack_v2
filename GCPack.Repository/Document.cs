//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GCPack.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document()
        {
            this.DivisionCategories = new HashSet<DivisionCategory>();
            this.LogEvents = new HashSet<LogEvent>();
            this.ReadConfirmations = new HashSet<ReadConfirmation>();
            this.Signatures = new HashSet<Signature>();
            this.SystemCategories = new HashSet<SystemCategory>();
            this.Files = new HashSet<File>();
            this.JobPositionDocuments = new HashSet<JobPositionDocument>();
        }
    
        public int ID { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentNumber { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> EffeciencyDate { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public Nullable<System.DateTime> NextReviewDate { get; set; }
        public int AdministratorID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> DocumentAdminType { get; set; }
        public string Annotation { get; set; }
        public string Revision { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DivisionCategory> DivisionCategories { get; set; }
        public virtual State State { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogEvent> LogEvents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReadConfirmation> ReadConfirmations { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Signature> Signatures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemCategory> SystemCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobPositionDocument> JobPositionDocuments { get; set; }
    }
}
