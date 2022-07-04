using WebApiTemplate.Application.Common.Models;

namespace WebApiTemplate.Application.Identity.Users;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}