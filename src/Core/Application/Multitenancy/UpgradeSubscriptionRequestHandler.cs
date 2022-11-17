namespace Backend.Application.Multitenancy;

public class UpgradeSubscriptionRequestHandler : IRequestHandler<UpgradeSubscriptionRequest, string>
{
    private readonly ITenantService _tenantService;

    public UpgradeSubscriptionRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<string> Handle(UpgradeSubscriptionRequest request, CancellationToken cancellationToken) =>
        _tenantService.UpdateSubscription(request.TenantId, request.ExtendedExpiryDate);
}
