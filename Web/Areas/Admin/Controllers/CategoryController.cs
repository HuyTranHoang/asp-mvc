using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities;

namespace MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.RoleAdmin)]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [TempData] public string? SuccessMessage { get; set; }

    public IActionResult Index(int? page, string? s, string? sortOrder)
    {
        var query = _unitOfWork.Category.GetAll().AsQueryable();

        // var query = _unitOfWork.CategoryRepository.Get(null, "DisplayOrder").AsQueryable();

        ViewBag.NameSortParam = sortOrder == SortData.NameAsc
            ? SortData.NameDesc
            : SortData.NameAsc;

        ViewBag.DisplayOrderSortParam = sortOrder == SortData.DisplayOrderAsc
            ? SortData.DisplayOrderDesc
            : SortData.DisplayOrderAsc;

        ViewBag.CreatedAtParam = sortOrder == SortData.CreatedAtAsc
            ? SortData.CreatedAtDesc
            : SortData.CreatedAtAsc;

        query = query.ApplySortCategory(sortOrder);

        if (!string.IsNullOrEmpty(s))
        {
            s = s.Trim();
            query = query.Where(c => c.Name.Contains(s, StringComparison.OrdinalIgnoreCase));
        }

        var pageNumber = page ?? 1;
        var recsCount = query.Count();
        var pager = new Pager(recsCount, pageNumber);

        var recSkip = (pageNumber - 1) * pager.PageSize;

        var onePageOfCategories = query.Skip(recSkip).Take(pager.PageSize);

        ViewBag.Pager = pager;

        return View(onePageOfCategories);
    }

    public IActionResult Upsert(int? id)
    {
        Category category;

        if (id == null || id == 0)
        {
            category = new Category();
        }
        else
        {
            category = _unitOfWork.Category.GetById(id);
            if (category == null) return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(int id, [Bind("Id,Name, DisplayOrder")] Category category)
    {
        if (id != category.Id) return NotFound();

        if (!ModelState.IsValid) return View("upsert", category);

        if (id == 0)
        {
            Validation(category);
            _unitOfWork.Category.Insert(category);
            SuccessMessage = "New category added";
        }
        else
        {
            Validation(category, true);
            _unitOfWork.Category.Update(category);
            SuccessMessage = "Product updated";
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, int? page)
    {
        _unitOfWork.Category.Delete(id);
        if (_unitOfWork.Save() > 0) SuccessMessage = "Category deleted";

        var categories = _unitOfWork.Category.GetAll();
        var pageNumber = page ?? 1;
        if (!Pager.HasProductsOnPage(categories, pageNumber)) pageNumber -= 1;

        return RedirectToAction(nameof(Index), new { page = pageNumber });
    }

    private void Validation(Category category, bool isUpdate = false)
    {
        bool categoryNameExist, categoryDisplayOrderExist;
        if (!isUpdate)
        {
            categoryNameExist = _unitOfWork.Category
                .Get(c => c.Name == category.Name).Any();

            categoryDisplayOrderExist = _unitOfWork.Category
                .Get(c => c.Name == category.Name).Any();
        }
        else
        {
            categoryNameExist = _unitOfWork.Category
                .Get(c => c.Name == category.Name && c.Id != category.Id).Any();
            categoryDisplayOrderExist = _unitOfWork.Category
                .Get(c => c.DisplayOrder == category.DisplayOrder && c.Id != category.Id).Any();
        }

        if (categoryNameExist) ModelState.AddModelError("Name", "The category name already exist");

        if (categoryDisplayOrderExist)
            ModelState.AddModelError("DisplayOrder", "The category display order already exist");
    }
}