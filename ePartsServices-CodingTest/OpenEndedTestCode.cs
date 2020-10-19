using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ePartsServices_CodingTest;

/*
 * What we're looking for:
 * • Create an Api to asynchronously return a DTO of active products with more than 2 options, for a given Manufacturer (by Name) and Customer (by Name).
 * • Customers are only allowed to access Products if a relationship exists on ManufacturerCustomer table.
 * • Use multiplier service to calculate Price.
 * • DTO should have ProductId, ProductNumberCustomer, Price, Description, and ProductOptions. ProductOptions should contain OptionId and description.
 * • Order by least to most expensive.
 *
 * Notes: Cost_Price of a Product is not the same as 'Price' to be sent in the DTO. ProductMultipliersService is an interface that uses a product or set of products and returns it's calculated multiplier for a given product per customer. This customer-specific (decimal) multiplier can be multiplied by a given product's Cost_Price to find its 'Price' to be returned in the DTO.
*/

namespace Lemur.Application.ProductSlice
{

    #region ProductMultipliersService
    public interface IProductMultipliersService
    {
        #region Methods
        /// <summary>
        /// Gets a single IProductMultipliers for the given ID.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <returns>The product ID along with the multiplier</returns>
        Task<IProductMultipliers> InvokeAsync(int productId); //public async

        /// <summary>
        /// Gets a list of IProductMultipliers for the given IDs.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <returns>The product ID along with the multiplier</returns>
        Task<List<IProductMultipliers>> InvokeBulkAsync(List<int> productIds); //public async
        #endregion Methods
    }

    /// <summary>
    /// Interface for the multiplier for each product
    /// </summary>
    public interface IProductMultipliers
    {
        #region Properties
        /// <summary>
        /// ID of the product
        /// </summary>
        decimal ProductId { get; set; }

        /// <summary>
        /// The multiplier that will be combined with the cost price of the product to get the full price. 
        /// </summary>
        decimal TotalProductMultiplier { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// multiplier for each product
    /// </summary>
    public class TestProductMultipliers : IProductMultipliers
    {
        #region Properties

        /// <summary>
        /// ID of the product
        /// </summary>
        public decimal ProductId { get; set; }

        /// <summary>
        /// The multiplier that will be combined with the cost price of the product to get the full price. 
        /// </summary>
        public decimal TotalProductMultiplier { get; set; }

        #endregion Properties
    }

    public class TestProductMultipliersService : IProductMultipliersService
    {

        /// <summary>
        /// this is just a very quick and dirty to test with. I know it's not using await, it may cause it to now not be async. But I need to know the calc works.
        /// </summary>
        /// <param name="productId">id of product</param>
        /// <returns></returns>
        public async Task<IProductMultipliers> InvokeAsync(int productId)
        {
            return new TestProductMultipliers{ ProductId = productId,  TotalProductMultiplier = 2};

        }

        /// <summary>
        /// this is just a very quick and dirty to test with
        /// </summary>
        /// <param name="productIds">id of product</param>
        /// <returns></returns>
        public async Task<List<IProductMultipliers>> InvokeBulkAsync(List<int> productIds) //public async
        {
            var productMultiplers = new List<IProductMultipliers>();
            foreach(int id in productIds)
            {
                productMultiplers.Add(InvokeAsync(id).Result);
            }
            return productMultiplers;
        }
    }

    #endregion ProductMultipliersService

    #region ManufacturerProductsByCompanyService

    /// <summary>
    /// Interface for the service to return the DTOs with product info
    /// </summary>
    public interface IManufacturerProductsByCompanyService
    {
        // methods
        Task<List<DTO>> InvokeAsync(string manufacturerName, string customerName); //public async
    }

    /// <summary>
    /// service to return the DTOs with product info
    /// </summary>
    public class ManufacturerProductsByCompanyService : IManufacturerProductsByCompanyService
    {

        #region Fields
        IProductMultipliersService _prodMultiplerService; //Product multipler service to get the multiplier to calculate the price.
        #endregion Fields

        #region Methods
        public ManufacturerProductsByCompanyService(IProductMultipliersService productMultipliersService)
        {
            _prodMultiplerService = productMultipliersService; //This one's odd, setting it like this in the constructor is good for dependency injection
                                                               //But it's also being set in the initial ProductController. As currently written I'd remove it
                                                               //from the ProductController.
        }

        /// <summary>
        /// returns list of products for give manufacturer and customer where the product is active and has more than 2 options.
        /// TODO: Personally would have it ID based, as manufacturers and customers could have similar names. Probably would bring this up if requirements or design stated this to understand reasoning.
        /// </summary>
        /// <param name="manufacturerName">name of the manufacturer</param>
        /// <param name="customerName">name of customer</param>
        /// <returns>list of product dto for give manufacturer and customer where the product is active and has more than 2 options</returns>
        public async Task<List<DTO>> InvokeAsync(string manufacturerName, string customerName)
        {
            var tasks = new List<Task>(); //will be used to loop through and get prices
            var products = new List<DTO>();
            try
            {
                using (var context = new CodingTestContext())
                {

                    products = await (from p in context.ProductTables
                                      from CustTable in p.ManufacturerTables.ManufacturerCustomerTable
                                      where CustTable.CustomerTable.Cust_Name == customerName //for given customer
                                            && p.ManufacturerTables.Name == manufacturerName //for the given manufacturer
                                            && p.Product_Active //where product is active
                                            && p.ProductOptionsTables.Count >= 2 //and there's more than 2 options
                                      select new DTO
                                      {
                                          ProductId = p.Product_ID,
                                          Description = p.Product_Description,
                                          ProductOptions = p.ProductOptionsTables.ToList(),
                                          ProductNumberCustomer = p.Product_Number_Custom,
                                          Price = p.Cost_Price //will be multiplied in next step. Maybe I could have made an anon type instead and a method to switch from anon to actual DTO. But at the same time that felt somewhat redundent? Certainly a case where a code review is appreciated. 
                                      }).ToListAsync();

                    //populate the list of tasks to be run. 
                    foreach (var prod in products)
                    {
                        tasks.Add(Task.Run(async () => { await getPrice(prod); }));
                    }

                    tasks.Add(Task.Run(() => { products = products.OrderByDescending(dto => dto.Price).ToList(); }));

                    //run tasks and return the final products (Now with pricing!)
                    var t = Task.WhenAll(tasks);

                    await t;
                }
            }
            catch(Exception)
            {
                throw;
            }
            return products;

        }

        /// <summary>
        /// Quick task to get the price from the multiplier service.
        /// TODO: Probably would make it more usable to refactor to return the price specifically instead of DTO with the price. This way has
        /// the main method be a little cleaner though.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns> DTO with price </returns>
        private async Task getPrice(DTO dto)
        {
            try
            {
                var pmPrice = await _prodMultiplerService.InvokeAsync(dto.ProductId);

                if (pmPrice != null)
                    dto.Price = dto.Price * pmPrice.TotalProductMultiplier;
            }
            catch (Exception) { throw; }


        }

        #endregion Methods
    }

    #endregion ManufacturerProductsByCompanyService

    #region ProductController

    /// <summary>
    /// Gets the info from the service and returns
    /// </summary>
    [ApiController]
    public class ProductController
    {

        private readonly DbContext _alpsContext;
        //private readonly IProductMultipliersService _multiplierService; 
        private readonly IManufacturerProductsByCompanyService _manuProductsByComnpanyService;

        public ProductController(DbContext alpsContext, IManufacturerProductsByCompanyService manuProductsByComnpanyService)
        {
            //_multiplierService = multiplierService;
            _manuProductsByComnpanyService = manuProductsByComnpanyService;
            _alpsContext = alpsContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manufacturerName">name of the manufacturer</param>
        /// <param name="customerName">name of customer</param>
        /// <returns>A list of data transfer objects with the product and pricing info</returns>
        [HttpGet]
        public List<DTO> GetManufacturerProductsByCustomer([FromQuery] string manufacturerName, string customerName)
        {
            //First check for nulls. Not sure exactly how your invalid or bad requests are handled, but would follow whatever your error handling is. 
            if(String.IsNullOrEmpty(manufacturerName))
            {
                throw new ArgumentNullException("manufacturerName");
            }
            if(String.IsNullOrEmpty(customerName))
            {
                throw new ArgumentNullException("customerName");
            }

            return _manuProductsByComnpanyService.InvokeAsync(manufacturerName, customerName).Result;
        }
    }

    #endregion ProductController

    #region DTO

    /// <summary>
    /// Data Transfer object with product info. 
    /// </summary>
    public class DTO
    {
        public int ProductId { get; set; }
        public string ProductNumberCustomer { get; set; }  //used to be name in sample code but requested the Product Number instead.
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<ProductOptionsTable> ProductOptions { get; set; }  //Don't know how much I like hard typing this. Maybe could have kept as a object
    }
    #endregion DTO
}
