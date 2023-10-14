using System.Linq.Expressions;
using Mvc.Models;

namespace Mvc.Utilities;

public static class MyExtensions
{
    public static IQueryable<Category> ApplySortCategory(this IQueryable<Category> query, string? sortOrder)
    {
        switch (sortOrder)
        {
            case SortData.NameAsc:
                return query.OrderBy(c => c.Name);
            case SortData.NameDesc:
                return query.OrderByDescending(c => c.Name);
            case SortData.DisplayOrderAsc:
                return query.OrderBy(c => c.DisplayOrder);
            case SortData.DisplayOrderDesc:
                return query.OrderByDescending(c => c.DisplayOrder);
            case SortData.CreatedAtAsc:
                return query.OrderBy(c => c.CreatedAt);
            case SortData.CreatedAtDesc:
                return query.OrderByDescending(c => c.CreatedAt);
            default:
                return query.OrderBy(c => c.Id);
        }
    }

    public static IQueryable<Product> ApplySortProduct(this IQueryable<Product> query, string? sortOrder)
    {
        switch (sortOrder)
        {
            case SortData.NameAsc:
                return query.OrderBy(p => p.Name);
            case SortData.NameDesc:
                return query.OrderByDescending(p => p.Name);
            case SortData.PriceAsc:
                return query.OrderBy(p => p.Price);
            case SortData.PriceDesc:
                return query.OrderByDescending(p => p.Price);
            case SortData.CreatedAtAsc:
                return query.OrderBy(p => p.CreatedAt);
            case SortData.CreatedAtDesc:
                return query.OrderByDescending(p => p.CreatedAt);
            default:
                return query.OrderBy(p => p.Id);
        }
    }

    public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool isDescending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        string methodName = isDescending ? "OrderByDescending" : "OrderBy";
        var expression = Expression.Call(typeof(Queryable), methodName, new[] { typeof(T), property.Type },
            source.Expression, Expression.Quote(lambda));
        return source.Provider.CreateQuery<T>(expression);
    }
}