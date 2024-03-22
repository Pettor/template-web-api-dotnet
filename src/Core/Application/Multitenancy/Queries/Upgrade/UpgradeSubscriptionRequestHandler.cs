using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Upgrade;

public class UpgradeSubscriptionRequestHandler(ITenantService tenantService) : IRequestHandler<UpgradeSubscriptionRequest, string>
{
    public Task<string> Handle(UpgradeSubscriptionRequest request, CancellationToken cancellationToken) =>
        tenantService.UpdateSubscription(request.TenantId, request.ExtendedExpiryDate);
}
