using Backend.Application.Common.Validation;

namespace Backend.Application.Multitenancy.Queries.Upgrade;

public class UpgradeSubscriptionRequestValidator : CustomValidator<UpgradeSubscriptionRequest>
{
    public UpgradeSubscriptionRequestValidator() => RuleFor(t => t.TenantId).NotEmpty();
}
