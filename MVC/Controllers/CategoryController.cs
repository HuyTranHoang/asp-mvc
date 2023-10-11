using Microsoft.AspNetCore.Mvc;
using MVC.Dao;
using MVC.Models;


namespace MVC.Controllers;

public class CategoryController : Controller
{
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly ILogger<CategoryController> _logger;
    [TempData] public string? SuccessMessage { get; set; }

    public CategoryController(IGenericRepository<Category> categoryRepository, ILogger<CategoryController> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public IActionResult Index(int? page, string? s)
    {
        var query = _categoryRepository.GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(s))
        {
            query = query.Where(c => c.Name.Contains(s));
        }

        var pageNumber = page ?? 1;
        var pageSize = 5;
        var recsCount = query.Count();
        var pager = new Pager(recsCount, pageNumber, pageSize);

        var recSkip = (pageNumber - 1) * pageSize;

        var onePageOfCategories = query.OrderBy(c => c.Id)
            .Skip(recSkip)
            .Take(pager.PageSize);

        ViewBag.Pager = pager;

        return View(onePageOfCategories);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add([Bind("Name, DisplayOrder")] Category category)
    {
        bool checkCategoryNameExist = _categoryRepository.GetAll().Any(c => c.Name == category.Name);
        bool checkCategoryDisplayOrderExist =
            _categoryRepository.GetAll().Any(c => c.DisplayOrder == category.DisplayOrder);

        if (checkCategoryNameExist)
        {
            ModelState.AddModelError("Name", "The category name already exist");
        }

        if (checkCategoryDisplayOrderExist)
        {
            ModelState.AddModelError("DisplayOrder", "The category display order already exist");
        }

        if (ModelState.IsValid)
        {
            _categoryRepository.Insert(category);
            _categoryRepository.Save();

            SuccessMessage = "New category added";
            return RedirectToAction("Index");
        }

        return View("create");
    }

    public IActionResult Edit(int id)
    {
        var category = _categoryRepository.GetById(id);

        if (category != null)
        {
            return View(category);
        }

        return BadRequest();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id, [Bind("Name, DisplayOrder")] Category updateCategory)
    {
        var category = _categoryRepository.GetById(id);

        if (category != null)
        {
            bool checkCategoryNameExist = _categoryRepository.GetAll()
                .Any(c => c.Name == updateCategory.Name && c.Id != updateCategory.Id);
            bool checkCategoryDisplayOrderExist = _categoryRepository.GetAll()
                .Any(c => c.DisplayOrder == updateCategory.DisplayOrder && c.Id != updateCategory.Id);

            if (checkCategoryNameExist)
            {
                ModelState.AddModelError("Name", "The category name already exist");
            }

            if (checkCategoryDisplayOrderExist)
            {
                ModelState.AddModelError("DisplayOrder", "The category display order already exist");
            }

            category.Name = updateCategory.Name;
            category.DisplayOrder = updateCategory.DisplayOrder;
            _categoryRepository.Update(category);

            if (_categoryRepository.Save() > 0)
            {
                SuccessMessage = "Category updated";
                return RedirectToAction("Index");
            }
        }

        return BadRequest();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, string? page)
    {
        _categoryRepository.Delete(id);

        if (_categoryRepository.Save() > 0)
        {
            SuccessMessage = "Category deleted";
            return RedirectToAction("Index", new{page});
        }

        return BadRequest();
    }
}