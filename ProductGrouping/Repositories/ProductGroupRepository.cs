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
    public class ProductGroupRepository: IProductGroupRepository
    {
        private readonly Context _context;
        private readonly ILogger<ProductGroupRepository> _logger;

        public ProductGroupRepository(Context context,
                                      ILogger<ProductGroupRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<ProductGroup> Get(Guid? id)
        {

            return _context.ProductGroups
                           .FindAsync(id);
        }

        public async Task<ProductGroup> Get(string productReference)
        {
            return await _context.ProductGroups
                                 .Where(p => p.ProductReference == productReference)
                                 .FirstOrDefaultAsync();
                        
        }

        public async Task<IEnumerable<ProductGroup>> GetMany()
        {
             return await _context.ProductGroups
                                  .ToListAsync();
        }

        public async Task<IEnumerable<ProductGroup>> GetMany(Expression<Func<ProductGroup, bool>> where)
        {
            return await _context.ProductGroups
                                 .Where(where)
                                 .ToListAsync();
        }

        public IQueryable<ProductGroup> GetMany(Expression<Func<ProductGroup, bool>> where, Expression<Func<ProductGroup, string>> orderBy, bool ascending)
        {
            if (ascending)
            {
                return _context.ProductGroups
                               .Where(where)
                               .OrderBy(orderBy);
            }
            else
            {
                return _context.ProductGroups
                               .Where(where)
                               .OrderByDescending(orderBy);
            }            
        }

        public async Task Post(ProductGroup productGroup)
        {
            _context.Add(productGroup);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task Put(ProductGroup productGroup)
        {
            _context.Update(productGroup);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task Delete(ProductGroup productGroup)
        {
            _context.ProductGroups
                    .Remove(productGroup);

            await _context.SaveChangesAsync();

            return;
        }

        public Task<bool> Exists(Guid id)
        {
            return _context.ProductGroups
                           .AnyAsync();
        }
    }
}
