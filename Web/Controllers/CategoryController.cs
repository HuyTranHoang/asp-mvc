using Microsoft.AspNetCore.Mvc;
using Mvc.DataAccess.Repository.IRepository;
using MVC.Models;
using Mvc.Utilities;
using Mvc.Utilities.Enum;

namespace MVC.Controllers;

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    [TempData] public string? SuccessMessage { get; set; }

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index(int? page, string? s, CategorySortOrder? sortOrder)
    {
        var query = _unitOfWork.Category.GetAll().AsQueryable();

        // var query = _unitOfWork.CategoryRepository.Get(null, "DisplayOrder").AsQueryable();

        ViewBag.NameSortParam = sortOrder == CategorySortOrder.NameAsc
            ? CategorySortOrder.NameDesc
            : CategorySortOrder.NameAsc;

        ViewBag.DisplayOrderSortParam = sortOrder == CategorySortOrder.DisplayOrderAsc
            ? CategorySortOrder.DisplayOrderDesc
            : CategorySortOrder.DisplayOrderAsc;

        ViewBag.CreatedAtParam = sortOrder == CategorySortOrder.CreatedAtAsc
            ? CategorySortOrder.CreatedAtDesc
            : CategorySortOrder.CreatedAtAsc;

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


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Store([Bind("Id,Name, DisplayOrder")] Category category)
    {
        Validation(category);

        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Insert(category);
            _unitOfWork.Save();

            SuccessMessage = "New category added";
            return RedirectToAction("Index");
        }

        return View("create");
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = _unitOfWork.Category.GetById(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id, [Bind("Id,Name, DisplayOrder")] Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
            SuccessMessage = "Product updated";
            return RedirectToAction("Index");
        }

        return View("edit", category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, int? page)
    {
        _unitOfWork.Category.Delete(id);
        if (_unitOfWork.Save() > 0)
        {
            SuccessMessage = "Category deleted";
        }

        var categories = _unitOfWork.Category.GetAll();
        var pageNumber = page ?? 1;
        if (!Pager.HasProductsOnPage(categories, pageNumber))
        {
            pageNumber -= 1;
        }

        return RedirectToAction("Index", new { page = pageNumber });
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

        if (categoryNameExist)
        {
            ModelState.AddModelError("Name", "The category name already exist");
        }

        if (categoryDisplayOrderExist)
        {
            ModelState.AddModelError("DisplayOrder", "The category display order already exist");
        }
    }
}