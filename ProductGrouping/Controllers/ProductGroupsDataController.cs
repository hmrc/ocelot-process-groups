using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductGroupsDataController : ControllerBase
    {
        private readonly ILogger<ProductGroupsDataController> _logger;
        private readonly IProductGroupRepository _productGroupRepository;

        public ProductGroupsDataController(ILogger<ProductGroupsDataController> logger,
                                           IProductGroupRepository productGroupRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
        }

        // GET: api/ProductGroupsApi
        [HttpGet]
        public async Task<IEnumerable<ProductGroup>> GetProductGroups()
        {
            return await _productGroupRepository.GetMany();
        }

        // GET: api/ProductGroupsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductGroupById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productGroup = await _productGroupRepository.Get(id);

            if (productGroup == null)
            {
                return NotFound();
            }

            return Ok(productGroup);
        }

        // GET: api/ProductGroupsApi/GetbyProductReference?productReference=productReference
        [HttpGet(), Route("GetByProductReference")]
        public async Task<IActionResult> GetByProductReference(string productReference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productGroup = await _productGroupRepository.Get(productReference);

            if (productGroup == null)
            {
                return NotFound();
            }

            return Ok(productGroup);
        }

        // GET: api/ProductGroupsApi/GetByOwner?productOwner=productOwner
        [HttpGet(), Route("GetByProductOwner")]
        public async Task<IEnumerable<ProductGroup>> GetProductGroupsByProductOwner(string productOwner)
        {
            return await _productGroupRepository.GetMany(p => p.ProductOwner == productOwner);
        }

        // GET: api/ProductGroupsApi/GetByGroup?group=group
        [HttpGet(), Route("GetByGroup")]
        public async Task<IEnumerable<ProductGroup>> GetProductGroupsByGroup(string group)
        {
            return await _productGroupRepository.GetMany(p => p.Group == group);
        }

        // GET: api/ProductGroupsApi/GetBySite?site=site
        [HttpGet(), Route("GetBySite")]
        public async Task<IEnumerable<ProductGroup>> GetProductGroupsBySite(string site)
        {
            return await _productGroupRepository.GetMany(p => p.Site == site);
        }
    }
}