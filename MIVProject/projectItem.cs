//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIVProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class projectItem
    {
        public Nullable<int> quantity { get; set; }
        public Nullable<decimal> price { get; set; }
        public string quality { get; set; }
        public string description { get; set; }
        public int projectPosition { get; set; }
        public int project { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> shipDate { get; set; }
        public int item { get; set; }
        public int projectItemID { get; set; }
    
        public virtual item item1 { get; set; }
        public virtual project project1 { get; set; }
    }
}
