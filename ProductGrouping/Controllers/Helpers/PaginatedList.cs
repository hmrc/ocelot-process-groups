using Microsoft.EntityFrameworkCore;
using ProductGrouping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers.Helpers
{
    /// <summary>
    /// PaginatedList T class
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class PaginatedList<T> : List<T>, IPaginatedList
    {
        /// <summary>
        /// Current Page index
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Total Pages
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// PaginatedList
        /// </summary>
        /// <param name="items">Items</param>
        /// <param name="count">Count</param>
        /// <param name="pageIndex">PageIndex</param>
        /// <param name="pageSize">PageSize</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        /// <summary>
        /// HasPreviousPage
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        /// <summary>
        /// HasNextPage
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        /// <summary>
        /// Create PaginatedList async
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">PageSize, number of records per page</param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}