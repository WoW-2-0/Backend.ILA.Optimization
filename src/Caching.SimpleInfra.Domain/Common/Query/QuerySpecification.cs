using System.Linq.Expressions;
using Caching.SimpleInfra.Domain.Common.Caching;
using Caching.SimpleInfra.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Caching.SimpleInfra.Domain.Common.Query;

public class QuerySpecification<TEntity>(uint pageSize, uint pageToken) : CacheModel where TEntity : IEntity
{
    public List<Expression<Func<TEntity, bool>>> FilteringOptions { get; } = new();

    public List<(Expression<Func<TEntity, object>> KeySelector, bool IsAscending)>? OrderingOptions { get; } = new();

    public FilterPagination PaginationOptions { get; set; } = new(pageSize, pageToken);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        var test = FilteringOptions.First().ToString();
        
        FilteringOptions?.Order()
            .Aggregate(
                hashCode,
                (current, filter) =>
                {
                    current.Add(filter.ToString());
                    return current;
                }
            );
        
        OrderingOptions?.Order()
            .Aggregate(
                hashCode,
                (current, order) =>
                {
                    current.Add(order.Item1.ToString());
                    current.Add(order.IsAscending);
                    return current;
                }
            );

        hashCode.Add(PaginationOptions);

        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is QuerySpecification<TEntity> querySpecification && querySpecification.GetHashCode() == GetHashCode();
    }

    public override string CacheKey => $"{typeof(TEntity).Name}_{GetHashCode()}";
}