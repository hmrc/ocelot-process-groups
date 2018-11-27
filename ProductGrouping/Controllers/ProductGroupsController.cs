using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using ProductGrouping.Views.Helpers;
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

        public ProductGroupsController(ILogger<ProductGroupsController> logger,
                                       IProductGroupRepository productGroupRepository,
                                       IAuthRepository authRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
            _authRepository = authRepository;
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
                             p.Site.ToUpper().Contains(searchString.ToUpper()) ||
                             p.Group.ToUpper().Contains(searchString.ToUpper());
            }

            int pageSize = 20;
            var productGroupings = _productGroupRepository.GetMany(where, orderby, ascending);

            return View(await PaginatedList<ProductGroup>.CreateAsync(productGroupings.AsNoTracking(), page ?? 1, pageSize));            
        }

        // GET: ProductGroups/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productGroupRepository.Get(id);

            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
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
            if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.UserMessage = "You are not authorised to create a product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }

            if (ModelState.IsValid)
            {
                productGroup.Id = Guid.NewGuid();

                await _productGroupRepository.Post(productGroup);

                return RedirectToAction(nameof(Index));
            }

            return View(productGroup);
        }

        // GET: ProductGroups/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productGroupRepository.Get(id);

            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
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
            else if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.UserMessage = "You are not authorised to edit this product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productGroupRepository.Put(productGroup);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productGroupRepository.Exists(productGroup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(productGroup);
        }

        // GET: ProductGroups/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productGroup = await _productGroupRepository.Get(id);

            if (productGroup == null)
            {
                return NotFound();
            }

            return View(productGroup);
        }

        // POST: ProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productGroup = await _productGroupRepository.Get(id);

            if (!await _authRepository.IsAuthedRole(@User.Identity.Name.Substring(@User.Identity.Name.IndexOf(@"\") + 1)))
            {
                ViewBag.UserMessage = "You are not authorised to delete this product Group. Please contact your local customer relationship manager.";

                return View(productGroup);
            }

            await _productGroupRepository.Delete(productGroup);

            return RedirectToAction(nameof(Index));
        }
    }
}
