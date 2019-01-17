using Microsoft.AspNetCore.Mvc.Rendering;
using ProductGrouping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductGrouping.Interfaces
{
    /// <summary>
    /// Product group repository interface
    /// </summary>
    public interface IProductGroupRepository
    {
        /// <summary>
        /// Required Get function
        /// </summary>
        /// <param name="where">Where function</param>
        /// <returns>Task of Product Group</returns>
        Task<ProductGroup> Get(Expression<Func<ProductGroup, bool>> where);

        /// <summary>
        /// Required GetMany function
        /// </summary>
        /// <returns>Task IEnumerable ProductGroups</returns>
        Task<IEnumerable<ProductGroup>> GetMany();

        /// <summary>
        /// Required GetMany function
        /// </summary>
        /// <param name="where">Where function</param>
        /// <returns>Task IEnumerable ProductGroup</returns>
        Task<IEnumerable<ProductGroup>> GetMany(Expression<Func<ProductGroup, bool>> where);

        /// <summary>
        /// Required GetMany function
        /// </summary>
        /// <param name="where">Where function</param>
        /// <param name="orderBy">Orderby function</param>
        /// <returns></returns>
        IQueryable<ProductGroup> GetMany(Expression<Func<ProductGroup, bool>> where, Expression<Func<ProductGroup, string>> orderBy);

        /// <summary>
        /// Required GetSelectList function
        /// </summary>
        /// <returns>IEnumerable SelectListItem</returns>
        IEnumerable<SelectListItem> GetSelectList();

        /// <summary>
        /// Requred Post function
        /// </summary>
        /// <param name="productGroup">ProductGroup</param>
        /// <returns>Task</returns>
        Task Post(ProductGroup productGroup);

        /// <summary>
        /// Required Put function
        /// </summary>
        /// <param name="productGroup">ProductGroup</param>
        /// <returns>Task</returns>
        Task Put(ProductGroup productGroup);

        /// <summary>
        /// Required Delete function
        /// </summary>
        /// <param name="productGroup">Product Group</param>
        /// <returns>Task</returns>
        Task Delete(ProductGroup productGroup);

        /// <summary>
        /// Required Exists function
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task bool</returns>
        Task<bool> Exists(Guid id);
    }
}
