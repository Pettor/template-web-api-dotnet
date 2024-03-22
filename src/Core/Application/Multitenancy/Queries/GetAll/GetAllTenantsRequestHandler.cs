using Backend.Application.Multitenancy.Entities;
using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.GetAll;

public class GetAllTenantsRequestHandler : IRequestHandler<GetAllTenantsRequest, List<TenantDto>>
{
    private readonly ITenantService _tenantService;

    public GetAllTenantsRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<List<TenantDto>> Handle(GetAllTenantsRequest request, CancellationToken cancellationToken) =>
        _tenantService.GetAllAsync();
}
