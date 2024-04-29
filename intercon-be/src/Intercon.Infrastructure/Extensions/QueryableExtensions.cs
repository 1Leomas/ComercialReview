using Intercon.Domain.Pagination;
using System.Linq.Expressions;

namespace Intercon.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderUsing<T, TProp>(
        this IQueryable<T> query,
        Expression<Func<T, TProp>> expression,
        SortingDirection sortDirection = SortingDirection.Ascending)
    {
        return sortDirection is SortingDirection.Ascending
            ? query.OrderBy(expression)
            : query.OrderByDescending(expression);
    }
}