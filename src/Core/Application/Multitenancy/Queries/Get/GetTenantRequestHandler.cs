using Backend.Application.Multitenancy.Entities;
using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Get;

public class GetTenantRequestHandler(ITenantService tenantService)
    : IRequestHandler<GetTenantRequest, TenantDto>
{
    public Task<TenantDto> Handle(GetTenantRequest request, CancellationToken cancellationToken) =>
        tenantService.GetByIdAsync(request.TenantId);
}
