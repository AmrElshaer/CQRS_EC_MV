using System.Linq.Expressions;
using MongoDB.Driver.Linq;

namespace Application.Query.Common.Extensions;

public static class LinqExtensions
{
    public static IMongoQueryable<TSource> WhereIf<TSource>(this IMongoQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
    {
        if (condition)
            return (IMongoQueryable<TSource>) Queryable.Where(source, predicate);
        else
            return source;
    }
}
