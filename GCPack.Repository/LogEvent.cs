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
    
    public partial class LogEvent
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Text { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> LogType { get; set; }
        public Nullable<int> ResourceID { get; set; }
    
        public virtual User User { get; set; }
    }
}
