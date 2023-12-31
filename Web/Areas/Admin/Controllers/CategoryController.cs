﻿using Microsoft.AspNetCore.Authorization;
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

    public IActionResult Index()
    {
        var categories = _unitOfWork.Category.GetAll();

        return View(categories);
    }

    public IActionResult Upsert(int? id)
    {
        Category category;

        if (id is null or 0)
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

        Validation(category);


        if (!ModelState.IsValid) return View("upsert", category);

        if (id == 0)
        {
            _unitOfWork.Category.Insert(category);
            SuccessMessage = "New category added";
        }
        else
        {
            _unitOfWork.Category.Update(category);
            SuccessMessage = "Product updated";
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    private void Validation(Category category)
    {
        var isCreate = category.Id == 0;

        var categoryNameExist = _unitOfWork.Category
            .Get(isCreate
                ? c => c.Name == category.Name
                : c => c.Name == category.Name && c.Id != category.Id)
            .Any();

        var categoryDisplayOrderExist = _unitOfWork.Category
            .Get(isCreate
                ? c => c.DisplayOrder == category.DisplayOrder
                : c => c.DisplayOrder == category.DisplayOrder && c.Id != category.Id)
            .Any();

        if (categoryNameExist)
            ModelState.AddModelError("Name", "The category name already exist");

        if (categoryDisplayOrderExist)
            ModelState.AddModelError("DisplayOrder", "The category display order already exist");
    }


    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var categoryList = _unitOfWork.Category.GetAll();
        return Json(new { data = categoryList });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _unitOfWork.Category.Delete(id);
        return Json(_unitOfWork.Save() > 0 ? new { success = true, message = "Category deleted" } : new { success = false, message = "Error while deleting" });
    }

    #endregion
}