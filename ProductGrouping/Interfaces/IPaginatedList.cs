namespace ProductGrouping.Interfaces
{
    /// <summary>
    /// PaginatedList interface
    /// </summary>
    public interface IPaginatedList
    {
        /// <summary>
        /// Required bool HasNextPage
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Required bool HasPreviousPage
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Required int PageIndex
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Required int TotalPages
        /// </summary>      
        int TotalPages { get; }
    }
}
