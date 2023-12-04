namespace Nexus.Common.Contracts;

/// <summary>
/// Represents a paginated list
/// </summary>
/// <typeparam name="T">Type of the items in the list</typeparam>

public class PaginatedList<T>
{
    /// <summary>
    /// Gets the current page index
    /// </summary>
    public int PageIndex { get; private set; }

    /// <summary>
    /// Gets the number of items per page
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Gets the total number of items
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Gets the total number of pages
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// Gets the items of the currently viewed page
    /// </summary>
    public List<T> Items { get; private set; }


    /// <summary>
    /// Initializing new instance of the PaginatedList class.
    /// </summary>
    /// <param name="items">List of items in the current page</param>
    /// <param name="count">Total number of items</param>
    /// <param name="pageIndex">Current page index</param>
    /// <param name="pageSize">Size of the page</param>
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }

    /// <summary>
    /// Check if there is a previous page
    /// </summary>
    public bool HasPreviousPage => (PageIndex > 1);

    /// <summary>
    /// Check if there is a next page
    /// </summary>
    public bool HasNextPage => (PageIndex < TotalPages);

    /// <summary>
    /// Creates a PaginatedList from a source
    /// </summary>
    /// <param name="source">The IQueryable source</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pageSize">The page size</param>
    /// <returns>A new PaginatedList of given type</returns>
    public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
    {
        int count = source.Count();
        List<T> items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}