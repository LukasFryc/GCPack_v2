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
    
    public partial class ProjectDocument
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int DocumentID { get; set; }
    
        public virtual Document Document { get; set; }
        public virtual Project Project { get; set; }
    }
}
