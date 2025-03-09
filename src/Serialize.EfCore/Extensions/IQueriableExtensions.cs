using System.Linq;

/// <summary>
/// Extension methods for <see cref="IQueryable{T}"/>.
/// There is some bug with 'Skip' and 'Take' functions this should be a good workaround?
/// </summary>
public static class IQueriableExtensions
{
    public static IQueryable<T> Page<T>(this IQueryable<T> query, int size, int page = 0)
        => query.Skip(size * page).Take(size);

    public static IQueryable<T> Page<T>(this IOrderedQueryable<T> query, int size, int page = 0)
      => query.Skip(size * page).Take(size);
}