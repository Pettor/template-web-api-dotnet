using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Deactivate;

public class DeactivateTenantRequestHandler(ITenantService tenantService)
    : IRequestHandler<DeactivateTenantRequest, string>
{
    public Task<string> Handle(
        DeactivateTenantRequest request,
        CancellationToken cancellationToken
    ) => tenantService.DeactivateAsync(request.TenantId);
}
