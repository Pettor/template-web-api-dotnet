using System.Data;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Common.Contracts;
using Backend.Infrastructure.Persistence.Context;
using Dapper;
using Finbuckle.MultiTenant.EntityFrameworkCore;

namespace Backend.Infrastructure.Persistence.Repository;

public class DapperRepository(ApplicationDbContext dbContext) : IDapperRepository
{
    public async Task<IReadOnlyList<T>> QueryAsync<T>(
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default
    )
        where T : class, IEntity =>
        (await dbContext.Connection.QueryAsync<T>(sql, param, transaction)).AsList();

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default
    )
        where T : class, IEntity
    {
        if (!dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
        {
            sql = sql.Replace("@tenant", dbContext.TenantInfo.Id);
        }

        var entity = await dbContext.Connection.QueryFirstOrDefaultAsync<T>(
            sql,
            param,
            transaction
        );

        return entity ?? throw new NotFoundException(string.Empty);
    }

    public Task<T> QuerySingleAsync<T>(
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        CancellationToken cancellationToken = default
    )
        where T : class, IEntity
    {
        if (!dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
        {
            sql = sql.Replace("@tenant", dbContext.TenantInfo.Id);
        }

        return dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}
