using Ardalis.Specification;
using Backend.Domain.Catalog;

namespace Backend.Infrastructure.Catalog;

public class RandomBrandsSpec : Specification<Brand>
{
    public RandomBrandsSpec() =>
        Query.Where(b => !string.IsNullOrEmpty(b.Name) && b.Name.Contains("Brand Random"));
}