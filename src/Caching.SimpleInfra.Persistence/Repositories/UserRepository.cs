using System.Linq.Expressions;
using Caching.SimpleInfra.Domain.Entities;
using Caching.SimpleInfra.Persistence.Caching;
using Caching.SimpleInfra.Persistence.DataContexts;
using Caching.SimpleInfra.Persistence.Repositories.Interfaces;

namespace Caching.SimpleInfra.Persistence.Repositories;

public class UserRepository(IdentityDbContext dbContext, ICacheBroker cacheBroker) : EntityRepositoryBase<User, IdentityDbContext>(
    dbContext,
    cacheBroker
), IUserRepository
{
    public new IQueryable<User> Get(Expression<Func<User, bool>>? predicate = default, bool asNoTracking = false) =>
        base.Get(predicate, asNoTracking);

    public new ValueTask<User?> GetByIdAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(userId, asNoTracking, cancellationToken);

    public new ValueTask<User> CreateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.CreateAsync(user, saveChanges, cancellationToken);

    public new ValueTask<User> UpdateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(user, saveChanges, cancellationToken);

    public new ValueTask<User?> DeleteByIdAsync(Guid userId, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(userId, saveChanges, cancellationToken);
}