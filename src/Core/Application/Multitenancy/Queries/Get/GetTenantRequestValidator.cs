using Backend.Application.Common.Validation;

namespace Backend.Application.Multitenancy.Queries.Get;

public class GetTenantRequestValidator : CustomValidator<GetTenantRequest>
{
    public GetTenantRequestValidator() => RuleFor(t => t.TenantId).NotEmpty();
}
