using Backend.Application.Catalog.Products.Entities;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products.Specifications;

public class ProductByIdWithBrandSpec
    : Specification<Product, ProductDetailsDto>,
        ISingleResultSpecification<Product>
{
    public ProductByIdWithBrandSpec(Guid id) => Query.Where(p => p.Id == id).Include(p => p.Brand);
}
