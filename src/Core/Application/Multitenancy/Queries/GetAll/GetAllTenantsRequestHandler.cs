using Backend.Application.Multitenancy.Entities;
using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.GetAll;

public class GetAllTenantsRequestHandler(ITenantService tenantService) : IRequestHandler<GetAllTenantsRequest, List<TenantDto>>
{
    public Task<List<TenantDto>> Handle(GetAllTenantsRequest request, CancellationToken cancellationToken) =>
        tenantService.GetAllAsync();
}
