//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ePartsServices_CodingTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class ManufacturerCustomerTable
    {
        public int ManufacturerCustomer_ID { get; set; }
    
        public virtual ManufacturerTable Manufacturer_ID { get; set; }
        public virtual CustomerTable Cust_ID { get; set; }
        public virtual CustomerTable CustomerTable { get; set; }
        public virtual ManufacturerTable ManufacturerTables { get; set; }
    }
}
