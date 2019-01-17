using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductGrouping.Repositories
{
    /// <summary>
    /// Product group repository, handles all database queries
    /// </summary>
    public class ProductGroupRepository: IProductGroupRepository
    {
        private readonly Context _context;
        private readonly ILogger<ProductGroupRepository> _logger;

        /// <summary>
        /// Constructor for Product groups Repository
        /// </summary>
        /// <param name="context">Context dependency injected</param>
        /// <param name="logger">Logger dependency injected</param>
        public ProductGroupRepository(Context context,
                                      ILogger<ProductGroupRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get individual product group
        /// </summary>
        /// <param name="where">Where function</param>
        /// <returns>Task ProductGroup</returns>
        public async Task<ProductGroup> Get(Expression<Func<ProductGroup, bool>> where)
        {
            return await _context.ProductGroups
                                 .Include(p => p.Parent)
                                 .Where(where)
                                 .FirstOrDefaultAsync();

        }

        /// <summary>
        /// Get many product groups
        /// </summary>
        /// <returns>Task IEnumerable ProductGroup</returns>
        public async Task<IEnumerable<ProductGroup>> GetMany()
        {
             return await _context.ProductGroups
                                  .Include(p => p.Parent)
                                  .ToListAsync();
        }

        /// <summary>
        /// Get many product groups
        /// </summary>
        /// <param name="where">Where function</param>
        /// <returns>Task IEnumerable ProductGroup</returns>
        public async Task<IEnumerable<ProductGroup>> GetMany(Expression<Func<ProductGroup, bool>> where)
        {
            return await _context.ProductGroups
                                 .Include(p => p.Parent)
                                 .Where(where)
                                 .ToListAsync();
        }

        /// <summary>
        /// Get many product groups
        /// </summary>
        /// <param name="where">Where function</param>
        /// <param name="orderBy">Order by function</param>
        /// <returns>IQueryable ProductGroup</returns>
        public IQueryable<ProductGroup> GetMany(Expression<Func<ProductGroup, bool>> where, Expression<Func<ProductGroup, string>> orderBy)
        {            
            return _context.ProductGroups
                            .Include(p => p.Parent)
                            .Where(where)
                            .OrderBy(orderBy);                      
        }

        /// <summary>
        /// Creates select list
        /// </summary>
        /// <returns>IEnumerable SelectListItem of ProductGroup</returns>
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.ProductGroups
                           .Include(p => p.Parent)
                           .Select(p => new SelectListItem
                                    {
                                        Text = p.ProductReference,
                                        Value = p.Id.ToString()
                                    });
        }

        /// <summary>
        /// Create product group
        /// </summary>
        /// <param name="productGroup">Product Group</param>
        /// <returns>Task</returns>
        public async Task Post(ProductGroup productGroup)
        {
            _context.Add(productGroup);
            await _context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Update product group
        /// </summary>
        /// <param name="productGroup">Product Group</param>
        /// <returns>Task</returns>
        public async Task Put(ProductGroup productGroup)
        {
            _context.Update(productGroup);
            await _context.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// Delete product group
        /// </summary>
        /// <param name="productGroup">Product Group</param>
        /// <returns>Task</returns>
        public async Task Delete(ProductGroup productGroup)
        {
            _context.ProductGroups
                    .Remove(productGroup);

            await _context.SaveChangesAsync();

            return;
        }

        /// <summary>
        /// Does product group exist
        /// </summary>
        /// <param name="id">Guid product group id</param>
        /// <returns>bool</returns>
        public Task<bool> Exists(Guid id)
        {
            return _context.ProductGroups
                           .AnyAsync();
        }
    }
}
