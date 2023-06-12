namespace BIP.InternalCRM.Application.Extensions;

public static class EnumerableExtensions
{
    public static bool ContainsBy<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source
            .Select(predicate)
            .Any();
    }

    public static IEnumerable<T> Condition<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<IEnumerable<T>, IEnumerable<T>> func)
    {
        return condition
            ? func(source)
            : source;
    }

    public static IEnumerable<T> ConditionalWhere<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<T, bool> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }
    
    public static IEnumerable<T> UsePagination<T>(
        this IEnumerable<T> source,
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