using WebApiTemplate.Domain.Common.Contracts;

namespace WebApiTemplate.Application.Common.Persistence;

/// <summary>
/// The read-only repository for an aggregate root.
/// </summary>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}