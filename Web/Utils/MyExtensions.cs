using System.Linq.Expressions;
using MVC.Enum;
using MVC.Models;

namespace MVC.Utility;

public static class MyExtensions
{
    public static IQueryable<Category> ApplySortCategory(this IQueryable<Category> query, CategorySortOrder? sortOrder)
    {
        switch (sortOrder)
        {
            case CategorySortOrder.NameDesc:
                return query.OrderByDescending(c => c.Name);
            case CategorySortOrder.NameAsc:
                return query.OrderBy(c => c.Name);
            case CategorySortOrder.DisplayOrderAsc:
                return query.OrderBy(c => c.DisplayOrder);
            case CategorySortOrder.DisplayOrderDesc:
                return query.OrderByDescending(c => c.DisplayOrder);
            case CategorySortOrder.CreatedAtAsc:
                return query.OrderBy(c => c.CreatedAt);
            case CategorySortOrder.CreatedAtDesc:
                return query.OrderByDescending(c => c.CreatedAt);
            default:
                return query.OrderBy(c => c.Id);
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