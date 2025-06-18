using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Upgrade;

public class UpgradeSubscriptionRequestHandler(
    ITenantService tenantService,
    IValidator<UpgradeSubscriptionRequest> requestValidator
) : IRequestHandler<UpgradeSubscriptionRequest, string>
{
    public Task<string> Handle(
        UpgradeSubscriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        requestValidator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        return tenantService.UpdateSubscription(request.TenantId, request.ExtendedExpiryDate);
    }
}
