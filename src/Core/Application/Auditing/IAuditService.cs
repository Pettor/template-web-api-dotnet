using WebApiTemplate.Application.Common.Interfaces;

namespace WebApiTemplate.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
}