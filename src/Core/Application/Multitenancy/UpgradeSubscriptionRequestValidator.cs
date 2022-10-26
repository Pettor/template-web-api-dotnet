using WebApiTemplate.Application.Common.Validation;

namespace WebApiTemplate.Application.Multitenancy;

public class UpgradeSubscriptionRequestValidator : CustomValidator<UpgradeSubscriptionRequest>
{
    public UpgradeSubscriptionRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}