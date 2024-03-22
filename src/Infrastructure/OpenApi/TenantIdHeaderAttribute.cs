using Backend.Shared.Multitenancy;

namespace Backend.Infrastructure.OpenApi;

public class TenantIdHeaderAttribute() : SwaggerHeaderAttribute(MultitenancyConstants.TenantIdName,
    "Input your tenant Id to access this API",
    string.Empty,
    true);
