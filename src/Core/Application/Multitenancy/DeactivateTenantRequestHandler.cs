namespace Backend.Application.Multitenancy;

public class DeactivateTenantRequestHandler : IRequestHandler<DeactivateTenantRequest, string>
{
    private readonly ITenantService _tenantService;

    public DeactivateTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<string> Handle(DeactivateTenantRequest request, CancellationToken cancellationToken) =>
        _tenantService.DeactivateAsync(request.TenantId);
}
