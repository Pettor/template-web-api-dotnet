namespace Backend.Application.Common.Models;

public static class PaginationFilterExtensions
{
    public static bool HasOrderBy(this PaginationFilter filter) => filter.OrderBy?.Any() is true;
}
