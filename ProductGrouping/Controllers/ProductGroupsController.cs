using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using System;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers
{
    public class ProductGroupsController : Controller
    {
        private readonly ILogger<ProductGroupsController> _logger;
        private readonly IProductGroupRepository _productGroupRepository;

        public ProductGroupsController(ILogger<ProductGroupsController> logger,
                                       IProductGroupRepository productGroupRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
        }

        // GET: ProductGroups
        public async Task<IActionResult> Index()
        {
            return View(await _productGroupRepository.GetMany());
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

            await _productGroupRepository.Delete(productGroup);

            return RedirectToAction(nameof(Index));
        }
    }
}
