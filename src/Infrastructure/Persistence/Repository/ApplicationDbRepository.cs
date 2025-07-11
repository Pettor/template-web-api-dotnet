﻿using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Backend.Application.Common.Persistence;
using Backend.Domain.Common.Contracts;
using Backend.Infrastructure.Persistence.Context;
using Mapster;

namespace Backend.Infrastructure.Persistence.Repository;

// Inherited from Ardalis.Specification's RepositoryBase<T>
public class ApplicationDbRepository<T>(ApplicationDbContext dbContext)
    : RepositoryBase<T>(dbContext),
        IReadRepository<T>,
        IRepository<T>
    where T : class, IAggregateRoot
{
    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    protected override IQueryable<TResult> ApplySpecification<TResult>(
        ISpecification<T, TResult> specification
    ) => ApplySpecification(specification, false).ProjectToType<TResult>();
}
