using Ardalis.Specification;
using Backend.Application.Common.Persistence;
using Backend.Domain.Common.Contracts;
using Backend.Domain.Common.Events;

namespace Backend.Infrastructure.Persistence.Repository;

/// <summary>
/// The repository that implements IRepositoryWithEvents.
/// Implemented as a decorator. It only augments the Add,
/// Update and Delete calls where it adds the respective
/// EntityCreated, EntityUpdated or EntityDeleted event
/// before delegating to the decorated repository.
/// </summary>
public class EventAddingRepositoryDecorator<T>(IRepository<T> decorated) : IRepositoryWithEvents<T>
    where T : class, IAggregateRoot
{
    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityCreatedEvent.WithEntity(entity));
        return decorated.AddAsync(entity, cancellationToken);
    }

    public Task<IEnumerable<T>> AddRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = new()
    ) => decorated.AddRangeAsync(entities, cancellationToken);

    public Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityUpdatedEvent.WithEntity(entity));
        return decorated.UpdateAsync(entity, cancellationToken);
    }

    public Task<int> UpdateRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = new()
    ) => decorated.UpdateRangeAsync(entities, cancellationToken);

    public Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.DomainEvents.Add(EntityDeletedEvent.WithEntity(entity));
        return decorated.DeleteAsync(entity, cancellationToken);
    }

    public Task<int> DeleteRangeAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        foreach (var entity in entities)
        {
            entity.DomainEvents.Add(EntityDeletedEvent.WithEntity(entity));
        }

        return decorated.DeleteRangeAsync(entities, cancellationToken);
    }

    public Task<int> DeleteRangeAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        throw new NotImplementedException();
    }

    // The rest of the methods are simply forwarded.
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        decorated.SaveChangesAsync(cancellationToken);

    public Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        where TId : notnull => decorated.GetByIdAsync(id, cancellationToken);

    public Task<T?> FirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = new()
    ) => decorated.FirstOrDefaultAsync(specification, cancellationToken);

    public Task<TResult?> FirstOrDefaultAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = new()
    ) => decorated.FirstOrDefaultAsync(specification, cancellationToken);

    public Task<T?> SingleOrDefaultAsync(
        ISingleResultSpecification<T> specification,
        CancellationToken cancellationToken = new()
    ) => decorated.SingleOrDefaultAsync(specification, cancellationToken);

    public Task<TResult?> SingleOrDefaultAsync<TResult>(
        ISingleResultSpecification<T, TResult> specification,
        CancellationToken cancellationToken = new()
    ) => decorated.SingleOrDefaultAsync(specification, cancellationToken);

    public Task<List<T>> ListAsync(CancellationToken cancellationToken = default) =>
        decorated.ListAsync(cancellationToken);

    public Task<List<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    ) => decorated.ListAsync(specification, cancellationToken);

    public Task<List<TResult>> ListAsync<TResult>(
        ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = default
    ) => decorated.ListAsync(specification, cancellationToken);

    public Task<bool> AnyAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    ) => decorated.AnyAsync(specification, cancellationToken);

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        decorated.AnyAsync(cancellationToken);

    public IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification) =>
        decorated.AsAsyncEnumerable(specification);

    public Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    ) => decorated.CountAsync(specification, cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        decorated.CountAsync(cancellationToken);
}
