using Backend.Application.Multitenancy.Entities;

namespace Backend.Application.Multitenancy.Queries.Get;

public class GetTenantRequest : IRequest<TenantDto>
{
    public string TenantId { get; set; } = default!;

    public GetTenantRequest(string tenantId) => TenantId = tenantId;
}
