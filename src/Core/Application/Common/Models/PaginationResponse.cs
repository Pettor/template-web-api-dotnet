namespace Backend.Application.Common.Models;

public class PaginationResponse<T>(List<T> data, int count, int page, int pageSize)
{
    public List<T> Data { get; set; } = data;

    public int CurrentPage { get; set; } = page;

    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);

    public int TotalCount { get; set; } = count;

    public int PageSize { get; set; } = pageSize;

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;
}
