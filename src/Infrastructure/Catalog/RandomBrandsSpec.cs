using Ardalis.Specification;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Infrastructure.Catalog;

public class RandomBrandsSpec : Specification<Brand>
{
    public RandomBrandsSpec() =>
        Query.Where(b => !string.IsNullOrEmpty(b.Name) && b.Name.Contains("Brand Random"));
}