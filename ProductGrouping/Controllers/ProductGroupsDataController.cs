using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers
{
    /// <summary>
    /// API controller for Product Grouping to get product group data
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ProductGroupsDataController : ControllerBase
    {
        private readonly ILogger<ProductGroupsDataController> _logger;
        private readonly IProductGroupRepository _productGroupRepository;

        /// <summary>
        /// Constructor for Product groups data controller
        /// </summary>
        /// <param name="logger">Logger dependency injected</param>
        /// <param name="productGroupRepository">Product Group repository dependency injected</param>
        public ProductGroupsDataController(ILogger<ProductGroupsDataController> logger,
                                           IProductGroupRepository productGroupRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
        }
        
        /// <summary>
        /// Get all product groups
        /// </summary>
        /// <returns>IEnumerable of Product Group</returns>
        [HttpGet]
        public async Task<IEnumerable<ProductGroup>> GetProductGroups()
        {
            return (await _productGroupRepository.GetMany()).Where(p => !p.ParentId.HasValue);
        }

        /// <summary>
        /// Get individual product group by id
        /// </summary>
        /// <param name="id">Product groups id of type guid</param>
        /// <returns>Product Group</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductGroupById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productGroup = await _productGroupRepository.Get(p => p.Id == id);

            if (productGroup == null)
            {
                return NotFound();
            }

            return Ok(productGroup);
        }

        /// <summary>
        /// Get individual product group by product reference
        /// </summary>
        /// <param name="productReference">Product groups reference of type string</param>
        /// <returns>Product Group</returns>
        [HttpGet(), Route("GetByProductReference")]
        public async Task<IActionResult> GetByProductReference(string productReference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productGroup = await _productGroupRepository.Get(p => p.ProductReference == productReference);

            if (productGroup == null)
            {                
                return NotFound();
            }

            return Ok(productGroup);
        }

        /// <summary>
        /// Gets the product group by product reference, if doesnt exist gets parent product group based on Site+Lob/Site/HMRC
        /// </summary>
        /// <list type="number">
        ///     <item>  
        ///         <term>LOB and Site</term>  
        ///         <description>Where product reference = lob and its parent = site</description>  
        ///     </item>  
        ///     <item>  
        ///         <term>Site</term>  
        ///         <description>Where product reference = site</description>  
        ///     </item>  
        ///     <item>  
        ///         <term>HMRC</term>  
        ///         <description>Where product reference = HMRC</description>  
        ///     </item>  
        ///</list>
        /// <param name="productReference">Product groups reference of type string</param>
        /// <returns>Product Group</returns>
        [HttpGet(), Route("WhereAmI")]
        public async Task<ProductGroup> WhereAmI(string productReference)
        {
            ProductGroup productGroup;

            productGroup  = await _productGroupRepository.Get(p => p.ProductReference == productReference);                       

            if (productGroup != null)
            {
                return productGroup;
            }
            
            var referer = HttpContext.Request.Headers["Referer"].ToString();

            if (!String.IsNullOrWhiteSpace(referer))
            {
                var site = referer.Substring(8, referer.IndexOf('.') - 8).Replace("cc-", "");
                var lob = productReference.Substring(0, 3);

                productGroup = await _productGroupRepository.Get(p => p.ProductReference == lob &&
                                                                      p.Parent.ProductReference == site);

                if (productGroup != null)
                {
                    return productGroup;
                }

                productGroup = await _productGroupRepository.Get(p => p.ProductReference == site);

                if (productGroup != null)
                {
                    return productGroup;
                }
            }        

            productGroup = await _productGroupRepository.Get(p => p.ProductReference == "HMRC");

            return productGroup; 
        }

        /// <summary>
        /// Get individual product group by product owner
        /// </summary>
        /// <param name="productOwner">Product groups owner of type string</param>
        /// <returns>Product Group</returns>
        [HttpGet(), Route("GetByProductOwner")]
        public async Task<IEnumerable<ProductGroup>> GetProductGroupsByProductOwner(string productOwner)
        {
            return await _productGroupRepository.GetMany(p => p.ProductOwner == productOwner);
        }

        /// <summary>
        /// Get individual product group by product group
        /// </summary>
        /// <param name="group">Product groups parent reference of type string</param>
        /// <returns>Product Group</returns>
        [HttpGet(), Route("GetByGroup")]
        public async Task<IEnumerable<ProductGroup>> GetProductGroupsByGroup(string group)
        {
            return await _productGroupRepository.GetMany(p => p.Parent.ProductReference == group);
        }
    }
}