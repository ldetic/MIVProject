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
    
    public partial class supplyItem
    {
        public int supply { get; set; }
        public int item { get; set; }
        public int itemNumber { get; set; }
        public int quantity { get; set; }
        public Nullable<decimal> price { get; set; }
        public string quality { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> shipDate { get; set; }
        public int supplyItemID { get; set; }
    
        public virtual item item1 { get; set; }
        public virtual supplyHeader supplyHeader { get; set; }
    }
}
