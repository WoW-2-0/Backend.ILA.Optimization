using Caching.SimpleInfra.Domain.Common.Entities;
using Caching.SimpleInfra.Domain.Common.Query;

namespace Caching.SimpleInfra.Domain.Extensions;

public static class LinqExtensions
{
    public static IQueryable<TSource> ApplySpecification<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        source = source
            .ApplyPredicates(querySpecification)
            .ApplyOrdering(querySpecification)
            .ApplyPagination(querySpecification.Pagination);

        return source;
    }

    public static IEnumerable<TSource> ApplySpecification<TSource>(this IEnumerable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        source = source
            .ApplyPredicates(querySpecification)
            .ApplyOrdering(querySpecification)
            .ApplyPagination(querySpecification.Pagination);

        return source;
    }

    public static IQueryable<TSource> ApplyPredicates<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        querySpecification.Predicates.ForEach(predicate => source = source.Where(predicate));

        return source;
    }

    public static IEnumerable<TSource> ApplyPredicates<TSource>(this IEnumerable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        querySpecification.Predicates.ForEach(predicate => source = source.Where(predicate.Compile()));

        return source;
    }

    public static IQueryable<TSource> ApplyOrdering<TSource>(this IQueryable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        querySpecification.OrderByExpressions.ForEach(
            orderByExpression => source = orderByExpression.IsAscending
                ? source.OrderBy(orderByExpression.Item1)
                : source.OrderByDescending(orderByExpression.Item1)
        );

        return source;
    }

    public static IEnumerable<TSource> ApplyOrdering<TSource>(this IEnumerable<TSource> source, QuerySpecification<TSource> querySpecification)
        where TSource : IEntity
    {
        querySpecification.OrderByExpressions.ForEach(
            orderByExpression => source = orderByExpression.IsAscending
                ? source.OrderBy(orderByExpression.Item1.Compile())
                : source.OrderByDescending(orderByExpression.Item1.Compile())
        );

        return source;
    }

    public static IQueryable<TSource> ApplyPagination<TSource>(this IQueryable<TSource> source, FilterPagination paginationOptions)
    {
        return source.Skip((paginationOptions.PageToken - 1) * paginationOptions.PageSize).Take((int)paginationOptions.PageSize);
    }

    public static IEnumerable<TSource> ApplyPagination<TSource>(this IEnumerable<TSource> source, FilterPagination paginationOptions)
    {
        return source.Skip((paginationOptions.PageToken - 1) * paginationOptions.PageSize).Take((int)paginationOptions.PageSize);
    }
}