using System.Linq.Expressions;
using Mvc.Models;

namespace Mvc.Utilities;

public static class MyExtensions
{
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