//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestAPI.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        public int store_id { get; set; }
        public int product_id { get; set; }
        public Nullable<int> quantity { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
