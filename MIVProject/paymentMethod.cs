
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
    
public partial class paymentMethod
{

    public paymentMethod()
    {

        this.project = new HashSet<project>();

        this.supplyHeader = new HashSet<supplyHeader>();

    }


    public int paymentID { get; set; }

    public string name { get; set; }



    public virtual ICollection<project> project { get; set; }

    public virtual ICollection<supplyHeader> supplyHeader { get; set; }

}

}
