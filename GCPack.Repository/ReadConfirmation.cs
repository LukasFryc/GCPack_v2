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
    
    public partial class ReadConfirmation
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Nullable<System.DateTime> ReadDate { get; set; }
        public int DocumentID { get; set; }
        public int JobPositionID { get; set; }
        public string JobPositionName { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual Document Document { get; set; }
        public virtual User User { get; set; }
    }
}
