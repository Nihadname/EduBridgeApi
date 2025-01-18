
using Microsoft.EntityFrameworkCore;

public class PaginationDto<T> 
{
    public PaginationDto(List<T> items, int currentPage, int totalPages, int totalRecords, int pageSize)
    {
        Items = items;
        CurrentPage = currentPage;
        TotalPages = totalPages;
        TotalRecords = totalRecords;
        PageSize = pageSize;
    }

    public List<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; }

    public bool HasNext => CurrentPage < TotalPages;
    public bool HasPrev => CurrentPage > 1;


    public static async Task<PaginationDto<T>> Create(IQueryable<T> query, int page, int pageSize)
    {
        int totalCount = await query.CountAsync(); 
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PaginationDto<T>(items, page, totalPages, totalCount, pageSize);
    }
}
