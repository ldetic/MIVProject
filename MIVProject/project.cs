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
    
    public partial class project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public project()
        {
            this.projectItem = new HashSet<projectItem>();
            this.supplyHeader = new HashSet<supplyHeader>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public int paymentMethod { get; set; }
        public Nullable<System.DateTime> paymentDate { get; set; }
        public int deliveryMethod { get; set; }
        public Nullable<System.DateTime> deliveryDate { get; set; }
        public string description { get; set; }
        public string src { get; set; }
    
        public virtual deliveryMethod deliveryMethod1 { get; set; }
        public virtual paymentMethod paymentMethod1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<projectItem> projectItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<supplyHeader> supplyHeader { get; set; }
    }
}
