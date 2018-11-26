using ProductGrouping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductGrouping.Interfaces
{
    public interface IProductGroupRepository
    {
        Task<ProductGroup> Get(Guid? id);

        Task<ProductGroup> Get(string productReference);

        Task<IEnumerable<ProductGroup>> GetMany();

        Task<IEnumerable<ProductGroup>> GetMany(Expression<Func<ProductGroup, bool>> where);

        Task<IEnumerable<ProductGroup>> GetMany(Expression<Func<ProductGroup, bool>> where, Expression<Func<ProductGroup, string>> orderBy, bool ascending);

        Task Post(ProductGroup productGroup);

        Task Put(ProductGroup productGroup);

        Task Delete(ProductGroup productGroup);

        Task<bool> Exists(Guid id);
    }
}
