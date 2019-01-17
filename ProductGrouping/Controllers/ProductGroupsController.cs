using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrouping.Controllers.Helpers;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers
{
    /// <summary>
    /// Controller for MVC views to edit product groups
    /// </summary>
    public class ProductGroupsController : Controller
    {
        private readonly ILogger<ProductGroupsController> _logger;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ILegacyFileRepository _legacyFileRepository;

        /// <summary>
        /// Constructor for Product groups controller
        /// </summary>
        /// <param name="logger">Logger dependency injected</param>
        /// <param name="productGroupRepository">Product Group repository dependency injected</param>
        /// <param name="authRepository">Auth repository dependency injected</param>
        /// <param name="legacyFileRepository">Legacy file repository dependency injected</param>
        public ProductGroupsController(ILogger<ProductGroupsController> logger,
                                       IProductGroupRepository productGroupRepository,
                                       IAuthRepository authRepository,
                                       ILegacyFileRepository legacyFileRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
            _authRepository = authRepository;
            _legacyFileRepository = legacyFileRepository;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <param name="currentFilter">Current search value</param>
        /// <param name="searchString">New search value</param>
        /// <param name="page">Current page number being viewed</param>
        /// <returns>View Paginated list of product groups</returns>
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page)
        {
            Expression<Func<ProductGroup, bool>> where = p => p.Id != null;
            Expression<Func<ProductGroup, string>> orderby = p => p.ProductReference;       

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                where = p => p.ProductReference.ToUpper().Contains(searchString.ToUpper()) ||
                             p.ProductOwner.ToUpper().Contains(searchString.ToUpper()) ||
                             p.Parent.ProductReference.ToUpper().Contains(searchString.ToUpper());
            }

            int pageSize = 20;
       
            try
            {
                var productGroupings = _productGroupRepository.GetMany(where, orderby);

                return View(await PaginatedList<ProductGroup>.CreateAsync(productGroupings.AsNoTracking(), page ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Details view
        /// </summary>
        /// <param name="id">Product Group id</param>
        /// <returns>Details View</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var productGroup = await _productGroupRepository.Get(p => p.Id == id);

                if (productGroup == null)
                {
                    return NotFound();               
                }

                productGroup.Children = (List<ProductGroup>) await _productGroupRepository.GetMany(p => p.ParentId == productGroup.Id);

                return View(productGroup);
            }
            catch(Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }     
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <returns>Create View</returns>
        public IActionResult Create()
        {
            try
            {
                ViewBag.Products = _productGroupRepository.GetSelectList();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Post to create a product group
        /// </summary>
        /// <param name="productGroup">product group (Id,ProductReference,ProductOwner,ParentId)</param>
        /// <returns>Redirect to index on sucess</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductReference,ProductOwner,ParentId")] ProductGroup productGroup)
        {
            if (!@User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            else if (!ModelState.IsValid)
            {
                return View(productGroup);
            }
            else if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.Products = _productGroupRepository.GetSelectList();
                ViewBag.UserMessage = "You are not authorised to create a product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }

            productGroup.Id = Guid.NewGuid();          

            try
            {
                await _productGroupRepository.Post(productGroup);
                await _legacyFileRepository.Publish();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id">Product Group id</param>
        /// <returns>Edit View</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                ViewBag.Products = _productGroupRepository.GetSelectList();

                var productGroup = await _productGroupRepository.Get(p => p.Id == id);

                if (productGroup == null)
                {
                    return NotFound();
                }

                return View(productGroup);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///  Post to edit a product group
        /// </summary>
        /// <param name="id">Product Group id</param>
        /// <param name="productGroup">Product group (Id,ProductReference,ProductOwner,ParentId)</param>
        /// <returns>Redirect to index on sucess</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductReference,ProductOwner,ParentId")] ProductGroup productGroup)
        {
            if (id == default(Guid))
            {
                return NotFound();
            }
            else if (!@User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            else if (!ModelState.IsValid)
            {
                ViewBag.Products = _productGroupRepository.GetSelectList();

                return View(productGroup);
            }
            else if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.Products = _productGroupRepository.GetSelectList();
                ViewBag.UserMessage = "You are not authorised to edit this product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }
            
            try
            {
                if (await CheckPartents(id, productGroup.ParentId) || await CheckChildren(id, productGroup.Id))
                {
                    ViewBag.Products = _productGroupRepository.GetSelectList();
                    ViewBag.UserMessage = "Cant be own Ancestor.";

                    return View(productGroup);
                }

                await _productGroupRepository.Put(productGroup);                    
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _productGroupRepository.Exists(productGroup.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogCritical(500, ex.Message, ex);

                    return StatusCode(500, ex.Message);
                }
            }

            try
            {
                await _legacyFileRepository.Publish();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete View
        /// </summary>
        /// <param name="id">Product Group id</param>
        /// <returns>Delete View</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }          

            try
            {
                var productGroup = await _productGroupRepository.Get(p => p.Id == id);

                if (productGroup == null)
                {
                    return NotFound();
                }

                return View(productGroup);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Post to delete product group
        /// </summary>
        /// <param name="id">Product Group id</param>
        /// <returns>Redirect to index on sucess</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == default(Guid))
            {
                return NotFound();
            }
            else if (!@User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
           
            ProductGroup productGroup;

            try
            {
                productGroup = await _productGroupRepository.Get(p => p.Id == id);

                if (productGroup == null)
                {
                    return NotFound();
                }

                if ((await _productGroupRepository.GetMany(p => p.ParentId == productGroup.Id)).Any())
                {
                    ViewBag.UserMessage = "The product has chidren. Cannot be deleted at this time";

                    return View(productGroup);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }

            if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.UserMessage = "You are not authorised to delete this product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }                  

            try
            {
                await _productGroupRepository.Delete(productGroup);
                await _legacyFileRepository.Publish();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500, ex.Message);
            }
        }

        private async Task<bool> CheckChildren(Guid checkId, Guid parentId)
        {
            var children = await _productGroupRepository.GetMany(p => p.ParentId == parentId);
            foreach (var child in children)
            {
                if (child.Id == checkId)
                {
                    return true;
                }
                if (await CheckChildren(checkId, child.Id))
                {
                    return true;
                };
            }
            return false;
        }

        private async Task<bool> CheckPartents(Guid id, Guid? parentId)
        {
            if (parentId == null)
            {
                return false;
            }

            var parent = await _productGroupRepository.Get(p => p.Id == parentId);

            if (parent.Id == id)
            {
                return true;
            }
            if (parent.ParentId == null)
            {
                return false;
            }
            return await CheckPartents(id, parent.ParentId.Value);
        }
    }
}
