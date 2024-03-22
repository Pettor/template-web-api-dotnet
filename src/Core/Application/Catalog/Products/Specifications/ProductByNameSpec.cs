using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products.Specifications;

public class ProductByNameSpec : Specification<Product>, ISingleResultSpecification<Product>
{
    public ProductByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}
