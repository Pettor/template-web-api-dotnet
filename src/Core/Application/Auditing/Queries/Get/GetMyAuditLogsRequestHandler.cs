using Backend.Application.Auditing.Entities;
using Backend.Application.Auditing.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Auditing.Queries.Get;

public class GetMyAuditLogsRequestHandler(ICurrentUser currentUser, IAuditService auditService)
    : IRequestHandler<GetMyAuditLogsRequest, List<AuditDto>>
{
    public Task<List<AuditDto>> Handle(
        GetMyAuditLogsRequest request,
        CancellationToken cancellationToken
    ) => auditService.GetUserTrailsAsync(currentUser.GetUserId());
}
