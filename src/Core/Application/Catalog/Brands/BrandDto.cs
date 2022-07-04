using WebApiTemplate.Application.Common.Interfaces;

namespace WebApiTemplate.Application.Catalog.Brands;

public class BrandDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}