using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrouping.Controllers.Helpers;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers
{
    public class ProductGroupsController : Controller
    {
        private readonly ILogger<ProductGroupsController> _logger;
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ILegacyFileRepository _legacyFileRepository;

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

        // GET: ProductGroups
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? page)
        {
            Expression<Func<ProductGroup, bool>> where = p => p.Id != null;
            Expression<Func<ProductGroup, string>> orderby = p => p.ProductReference;
            var ascending = true;

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
                var productGroupings = _productGroupRepository.GetMany(where, orderby, ascending);

                return View(await PaginatedList<ProductGroup>.CreateAsync(productGroupings.AsNoTracking(), page ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }
        }

        // GET: ProductGroups/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var productGroup = await _productGroupRepository.Get(id);

                if (productGroup == null)
                {
                    return NotFound();               
                }

                return View(productGroup);
            }
            catch(Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }     
        }

        // GET: ProductGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductReference,ProductOwner,Group,Site")] ProductGroup productGroup)
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
                ViewBag.UserMessage = "You are not authorised to create a product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }

            productGroup.Id = Guid.NewGuid();

            try
            {
                await _productGroupRepository.Post(productGroup);               
            }
            catch(Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }            

            try
            {
                await _legacyFileRepository.Publish();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }
        }

        // GET: ProductGroups/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var productGroup = await _productGroupRepository.Get(id);

                if (productGroup == null)
                {
                    return NotFound();
                }

                return View(productGroup);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }            
        }

        // POST: ProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductReference,ProductOwner,Group,Site")] ProductGroup productGroup)
        {
            if (id != productGroup.Id)
            {
                return NotFound();
            }
            else if (!@User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            else if (!ModelState.IsValid)
            {
                return View(productGroup);
            }
            else if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.UserMessage = "You are not authorised to edit this product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }
            
            try
            {
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
                    return StatusCode(500);
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

                return StatusCode(500);
            }
        }

        // GET: ProductGroups/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }          

            try
            {
                var productGroup = await _productGroupRepository.Get(id);

                if (productGroup == null)
                {
                    return NotFound();
                }

                return View(productGroup);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }
        }

        // POST: ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
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
                productGroup = await _productGroupRepository.Get(id);

                if (productGroup == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }

            if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.UserMessage = "You are not authorised to delete this product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }

            try
            {
                await _productGroupRepository.Delete(productGroup);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }                    

            try
            {
                await _legacyFileRepository.Publish();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return StatusCode(500);
            }
        }
    }
}
