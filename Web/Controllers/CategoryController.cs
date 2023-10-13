using Microsoft.AspNetCore.Mvc;
using MVC.Enum;
using MVC.Models;
using MVC.Repository.IRepository;
using MVC.Utility;

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
        var query = _unitOfWork.CategoryRepository.GetAll().AsQueryable();

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

        if (sortOrder != null)
        {
            query = query.ApplySortCategory(sortOrder);
        }

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
    public IActionResult Store(Category category)
    {
        Validation(category);

        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepository.Insert(category);
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

        var category = _unitOfWork.CategoryRepository.GetById(id);
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
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Save();
            SuccessMessage = "Product updated";
            return RedirectToAction("Index");
        }

        return View("edit", category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, string? page)
    {
        _unitOfWork.CategoryRepository.Delete(id);
        if (_unitOfWork.Save() > 0)
        {
            SuccessMessage = "Category deleted";
        }
        return RedirectToAction("Index", new { page });
    }

    private void Validation(Category category, bool isUpdate = false)
    {
        bool categoryNameExist, categoryDisplayOrderExist;
        if (!isUpdate)
        {
            categoryNameExist = _unitOfWork.CategoryRepository
                .Get(c => c.Name == category.Name).Any();

            categoryDisplayOrderExist = _unitOfWork.CategoryRepository
                .Get(c => c.Name == category.Name).Any();
        }
        else
        {
            categoryNameExist = _unitOfWork.CategoryRepository
                .Get(c => c.Name == category.Name && c.Id != category.Id).Any();
            categoryDisplayOrderExist = _unitOfWork.CategoryRepository
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