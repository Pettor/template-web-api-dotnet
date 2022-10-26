using Backend.Application.Common.Models;

namespace Backend.Application.Identity.Users;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}