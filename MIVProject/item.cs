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
    
    public partial class item
    {
        public item()
        {
            this.projectItem = new HashSet<projectItem>();
            this.supplyItem = new HashSet<supplyItem>();
        }
    
        public int itemID { get; set; }
        public string name { get; set; }
        public int subcategory { get; set; }
        public string description { get; set; }
        public string unitOfMeasure { get; set; }
        public Nullable<double> quantity { get; set; }
        public Nullable<bool> visible { get; set; }
    
        public virtual itemSubCategory itemSubCategory { get; set; }
        public virtual ICollection<projectItem> projectItem { get; set; }
        public virtual ICollection<supplyItem> supplyItem { get; set; }
    }
}
