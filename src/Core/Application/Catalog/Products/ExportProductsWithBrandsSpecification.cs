using WebApiTemplate.Application.Common.Specification;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Application.Catalog.Products;

public class ExportProductsWithBrandsSpecification : EntitiesByBaseFilterSpec<Product, ProductExportDto>
{
    public ExportProductsWithBrandsSpecification(ExportProductsRequest request)
        : base(request) =>
        Query
            .Include(p => p.Brand)
            .Where(p => p.BrandId.Equals(request.BrandId!.Value), request.BrandId.HasValue)
            .Where(p => p.Rate >= request.MinimumRate!.Value, request.MinimumRate.HasValue)
            .Where(p => p.Rate <= request.MaximumRate!.Value, request.MaximumRate.HasValue);
}