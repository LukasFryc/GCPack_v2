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
    
    public partial class JobPositionDocument
    {
        public int JobPositionDocumentId { get; set; }
        public int DocumentId { get; set; }
        public int JobPositionId { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual JobPosition JobPosition { get; set; }
        public virtual Document Document { get; set; }
    }
}
