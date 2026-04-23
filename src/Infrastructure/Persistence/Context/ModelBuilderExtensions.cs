using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Backend.Infrastructure.Persistence.Context;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder AppendGlobalQueryFilter<TInterface>(
        this ModelBuilder modelBuilder,
        Expression<Func<TInterface, bool>> filter
    )
    {
        var filterKey = typeof(TInterface).Name;

        var entities = modelBuilder
            .Model.GetEntityTypes()
            .Where(e =>
                e.BaseType is null && e.ClrType.GetInterface(typeof(TInterface).Name) is not null
            )
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            var parameterType = Expression.Parameter(modelBuilder.Entity(entity).Metadata.ClrType);
            var filterBody = ReplacingExpressionVisitor.Replace(
                filter.Parameters.Single(),
                parameterType,
                filter.Body
            );

            modelBuilder
                .Entity(entity)
                .HasQueryFilter(filterKey, Expression.Lambda(filterBody, parameterType));
        }

        return modelBuilder;
    }
}
