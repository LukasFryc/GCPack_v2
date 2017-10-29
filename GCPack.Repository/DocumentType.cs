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
    
    public partial class DocumentType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DocumentType()
        {
            this.Documents = new HashSet<Document>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> AdministratorID { get; set; }
        public Nullable<int> AuthorizinOfficerID { get; set; }
        public Nullable<int> OrderBy { get; set; }
        public int ValidityInYears { get; set; }
        public bool IsRequiredFillListOfPersons { get; set; }
        public bool AutomaticNumberingOfDocuments { get; set; }
        public string NumberingOfDocumentPrefix { get; set; }
        public string NumberingOfDocumentSeparator { get; set; }
        public Nullable<byte> NumberingOfDocumentLength { get; set; }
        public Nullable<int> LastNumberOfDocument { get; set; }
        public Nullable<int> DocumentEfficiencyDays { get; set; }
        public Nullable<int> InformedInDays { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document> Documents { get; set; }
        public virtual User User { get; set; }
    }
}
