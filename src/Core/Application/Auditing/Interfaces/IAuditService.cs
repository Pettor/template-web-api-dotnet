using Backend.Application.Auditing.Entities;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Auditing.Interfaces;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
}
