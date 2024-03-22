using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Active;

public class ActivateTenantRequestHandler(ITenantService tenantService) : IRequestHandler<ActivateTenantRequest, string>
{
    public Task<string> Handle(ActivateTenantRequest request, CancellationToken cancellationToken) =>
        tenantService.ActivateAsync(request.TenantId);
}
