﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CodingTestContext : DbContext
    {
        public CodingTestContext()
            : base("name=CodingTestContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ProductTable> ProductTables { get; set; }
        public virtual DbSet<ProductOptionsTable> ProductOptionsTables { get; set; }
        public virtual DbSet<CustomerTable> CustomerTables { get; set; }
        public virtual DbSet<ManufacturerTable> ManufacturerTables { get; set; }
        public virtual DbSet<ManufacturerCustomerTable> ManufacturerCustomerTables { get; set; }
    }
}
