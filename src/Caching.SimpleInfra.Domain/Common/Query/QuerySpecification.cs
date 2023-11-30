using System.Linq.Expressions;
using Caching.SimpleInfra.Domain.Common.Caching;
using Caching.SimpleInfra.Domain.Common.Entities;

namespace Caching.SimpleInfra.Domain.Common.Query;

public class QuerySpecification<TEntity>(int pageSize, int pageToken) : CacheModel where TEntity : IEntity
{
    public List<Expression<Func<TEntity, bool>>> Predicates { get; } = new();

    public List<(Expression<Func<TEntity, object>>, bool IsAscending)>? OrderByExpressions { get; } = new();

    public FilterPagination Pagination { get; set; } = new(pageSize, pageToken);

    public void AddPredicate(Expression<Func<TEntity, bool>> criteria)
    {
        Predicates.Add(criteria);
    }

    public void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, bool isAscending = false)
    {
        OrderByExpressions.Add((orderByExpression, isAscending));
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        
        hashCode.Add(Predicates);
        hashCode.Add(OrderByExpressions);
        hashCode.Add(Pagination);

        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not QuerySpecification<TEntity> querySpecification)
            return false;

        return Predicates.SequenceEqual(querySpecification.Predicates) && OrderByExpressions.SequenceEqual(querySpecification.OrderByExpressions) &&
               Pagination.Equals(querySpecification.Pagination);
    }

    public override string CacheKey => $"{typeof(TEntity).Name}_{GetHashCode()}";
}