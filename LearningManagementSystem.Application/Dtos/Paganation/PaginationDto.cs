
public class PaginationDto<T> 
{
    public PaginationDto(IEnumerable<T> items, int currentPage, int totalPages, int totalRecords, int pageSize)
    {
        Items = items;
        CurrentPage = currentPage;
        TotalPages = totalPages;
        TotalRecords = totalRecords;
        PageSize = pageSize;
    }

    public IEnumerable<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int PageSize { get; set; }

    public bool HasNext => CurrentPage < TotalPages;
    public bool HasPrev => CurrentPage > 1;


    public static async Task<PaginationDto<T>> Create(IEnumerable<T> items, int page, int take, int totalCount)
    {
        var totalPages = (int)Math.Ceiling((decimal)totalCount / take);
     items=  items.Skip((page - 1) * take)
                .Take(take);
        return new PaginationDto<T>(items, page, totalPages, totalCount, take);
    }
}
