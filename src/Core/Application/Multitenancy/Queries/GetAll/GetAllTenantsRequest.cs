using Backend.Application.Multitenancy.Entities;

namespace Backend.Application.Multitenancy.Queries.GetAll;

public class GetAllTenantsRequest : IRequest<List<TenantDto>> { }
