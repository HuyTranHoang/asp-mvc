using MVC.Controllers;
using MVC.Models;

namespace MVC.Utils;

public static class MyExtensions
{
    public static IQueryable<Category> ApplySortCategory(this IQueryable<Category> query,
        CategoryController.SortOrder? sortOrder)
    {
        switch (sortOrder)
        {
            case CategoryController.SortOrder.NameDesc:
                return query.OrderByDescending(c => c.Name);
            case CategoryController.SortOrder.NameAsc:
                return query.OrderBy(c => c.Name);
            case CategoryController.SortOrder.DisplayOrderAsc:
                return query.OrderBy(c => c.DisplayOrder);
            case CategoryController.SortOrder.DisplayOrderDesc:
                return query.OrderByDescending(c => c.DisplayOrder);
            default:
                return query.OrderBy(c => c.Id);
        }
    }
}