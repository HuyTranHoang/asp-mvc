using MVC.Models;
using MVC.Utility.Enum;

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
}