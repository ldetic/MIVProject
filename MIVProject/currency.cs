
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
    
public partial class currency
{

    public currency()
    {

        this.supplyHeader = new HashSet<supplyHeader>();

        this.project = new HashSet<project>();

    }


    public int currencyID { get; set; }

    public string name { get; set; }

    public string abbreviation { get; set; }



    public virtual ICollection<supplyHeader> supplyHeader { get; set; }

    public virtual ICollection<project> project { get; set; }

}

}
