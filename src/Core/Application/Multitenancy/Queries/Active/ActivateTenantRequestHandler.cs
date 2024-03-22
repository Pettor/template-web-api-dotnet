using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Active;

public class ActivateTenantRequestHandler : IRequestHandler<ActivateTenantRequest, string>
{
    private readonly ITenantService _tenantService;

    public ActivateTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<string> Handle(ActivateTenantRequest request, CancellationToken cancellationToken) =>
        _tenantService.ActivateAsync(request.TenantId);
}
