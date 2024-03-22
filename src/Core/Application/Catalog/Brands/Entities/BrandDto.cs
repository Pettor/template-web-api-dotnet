using Backend.Application.Common.Interfaces;

namespace Backend.Application.Catalog.Brands.Entities;

public class BrandDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
