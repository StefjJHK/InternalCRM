using System.Linq.Expressions;

namespace BIP.InternalCRM.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ConditionalWhere<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }
    
    public static IQueryable<T> UseIf<T>(
        this IQueryable<T> source,
        bool condition,
        Func<IQueryable<T>, IQueryable<T>> func)
    {
        return condition
            ? func(source)
            : source;
    }

    public static IQueryable<T> UsePagination<T>(
        this IQueryable<T> source,
        PaginationOptions options)
    {
        if (options == PaginationOptions.Empty)
        {
            return source;
        }

        return source
            .Skip((options.PageNumber - 1) * options.Count)
            .Take(options.Count);
    }
}