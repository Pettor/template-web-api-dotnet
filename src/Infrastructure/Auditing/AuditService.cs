using Backend.Application.Auditing.Entities;
using Backend.Application.Auditing.Interfaces;
using Backend.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Auditing;

public class AuditService(ApplicationDbContext context) : IAuditService
{
    public async Task<List<AuditDto>> GetUserTrailsAsync(Guid userId)
    {
        var trails = await context
            .AuditTrails.Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();

        return trails.Adapt<List<AuditDto>>();
    }
}
