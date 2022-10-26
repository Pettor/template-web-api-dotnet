using WebApiTemplate.Application.Common.Models;

namespace WebApiTemplate.Application.Catalog.Products;

public class ExportProductsRequest : BaseFilter, IRequest<Stream>
{
    public Guid? BrandId { get; set; }
    public decimal? MinimumRate { get; set; }
    public decimal? MaximumRate { get; set; }
}