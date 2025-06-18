using Backend.Application.Auditing.Entities;

namespace Backend.Application.Auditing.Queries.Get;

public class GetMyAuditLogsRequest : IRequest<List<AuditDto>> { }
