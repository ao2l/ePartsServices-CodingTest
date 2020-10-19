using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemur.Application.ProductSlice;
using System.Collections.Generic;

namespace UnitTestProjectePartsCodeTest
{
    [TestClass]
    public class UnitTest1
    {
       
        [TestMethod]
        public void RunForCustNullTest()
        {
            ProductController productController = new ProductController(new ePartsServices_CodingTest.CodingTestContext(),  new ManufacturerProductsByCompanyService(new TestProductMultipliersService()));
            List<DTO> dtolist = productController.GetManufacturerProductsByCustomer("test", "test");
            Assert.IsNotNull(dtolist);
        }

        [TestMethod]
        public void RunForCustOver1Test()
        {
            ProductController productController = new ProductController(new ePartsServices_CodingTest.CodingTestContext(), new ManufacturerProductsByCompanyService(new TestProductMultipliersService()));
            List<DTO> dtolist = productController.GetManufacturerProductsByCustomer("test", "test");
            Assert.IsTrue(dtolist.Count > 0);
        }

        [TestMethod]
        public void DidNotIncludeProductWithoutOptions()
        {
            ProductController productController = new ProductController(new ePartsServices_CodingTest.CodingTestContext(), new ManufacturerProductsByCompanyService(new TestProductMultipliersService()));
            List<DTO> dtolist = productController.GetManufacturerProductsByCustomer("test", "test");
            Assert.IsNull(dtolist.Find(d => d.ProductId == 5));  //id 5 does not have any options. 
        }

        [TestMethod]
        public void DidNotIncludeProductThatAreNotActive()
        {
            ProductController productController = new ProductController(new ePartsServices_CodingTest.CodingTestContext(), new ManufacturerProductsByCompanyService(new TestProductMultipliersService()));
            List<DTO> dtolist = productController.GetManufacturerProductsByCustomer("test", "test");
            Assert.IsNull(dtolist.Find(d => d.ProductId == 6));  //id 6 is not active
        }

        [TestMethod]
        public void DoesIncludeProperProduct()
        {
            ProductController productController = new ProductController(new ePartsServices_CodingTest.CodingTestContext(),  new ManufacturerProductsByCompanyService(new TestProductMultipliersService()));
            List<DTO> dtolist = productController.GetManufacturerProductsByCustomer("test", "test");
            Assert.IsNotNull(dtolist.Find(d => d.ProductId == 4)); 
        }

        [TestMethod]
        public void TestMultiplier()
        {
            ProductController productController = new ProductController(new ePartsServices_CodingTest.CodingTestContext(),  new ManufacturerProductsByCompanyService(new TestProductMultipliersService()));
            List<DTO> dtolist = productController.GetManufacturerProductsByCustomer("test", "test");
            Assert.IsNotNull(dtolist.Find(d => d.Price > 1));
        }

        [TestMethod]
        public void TestSort()
        {
            ProductController productController = new ProductController(new ePartsServices_CodingTest.CodingTestContext(),  new ManufacturerProductsByCompanyService(new TestProductMultipliersService()));
            List<DTO> dtolist = productController.GetManufacturerProductsByCustomer("test", "test");
            Assert.IsTrue(dtolist.FindIndex(dto => dto.ProductId == 7) == 0); //id 7 has a higher price
        }
    }
}
