
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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class supplyHeader
    {

        public supplyHeader()
        {

            this.supplyItem = new HashSet<supplyItem>();

        }


        public int supplyID { get; set; }

        public int paymentMethod { get; set; }

        public int deliveryMethod { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> paymentDate { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> deliveryDate { get; set; }

        public int supplier { get; set; }

        public Nullable<System.DateTime> date { get; set; }

        public Nullable<int> project { get; set; }

        public int status { get; set; }

        public int currency { get; set; }



        public virtual currency currency1 { get; set; }

        public virtual deliveryMethod deliveryMethod1 { get; set; }

        public virtual paymentMethod paymentMethod1 { get; set; }

        public virtual project project1 { get; set; }

        public virtual supplier supplier1 { get; set; }

        public virtual supplyStatus supplyStatus { get; set; }

        public virtual ICollection<supplyItem> supplyItem { get; set; }

    }

}
