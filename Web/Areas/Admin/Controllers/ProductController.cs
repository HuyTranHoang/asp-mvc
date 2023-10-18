using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities;

namespace MVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.RoleAdmin)]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    [TempData] public string? SuccessMessage { private get; set; }

    public IActionResult Index(int? page, string? s, string? sortOrder)
    {
        var query = _unitOfWork.Product
            .GetAll(includeProperties: "Category")
            .AsQueryable();

        ViewBag.NameSortParam = sortOrder == SortData.NameAsc
            ? SortData.NameDesc
            : SortData.NameAsc;

        ViewBag.PriceSortParam = sortOrder == SortData.PriceAsc
            ? SortData.PriceDesc
            : SortData.PriceAsc;

        ViewBag.CreatedAtParam = sortOrder == SortData.CreatedAtAsc
            ? SortData.CreatedAtDesc
            : SortData.CreatedAtAsc;

        query = query.ApplySortProduct(sortOrder);

        if (!string.IsNullOrEmpty(s))
        {
            s = s.Trim();
            query = query.Where(p => p.Name.Contains(s, StringComparison.OrdinalIgnoreCase));
        }

        var pageNumber = page ?? 1;
        var recsCount = query.Count();
        var pager = new Pager(recsCount, pageNumber);

        var recSkip = (pageNumber - 1) * pager.PageSize;

        var onePageOfProducts = query.Skip(recSkip).Take(pager.PageSize);

        ViewBag.Pager = pager;
        
        return View(onePageOfProducts);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _unitOfWork.Product.GetById(id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name");
        ViewData["CoverTypeId"] = new SelectList(_unitOfWork.CoverType.GetAll(), "Id", "Name");
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Store([Bind("Name,Author,ISBN,Price,Price50,Price100,Description,CategoryId,CoverTypeId")] Product product,
        IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwRootPath, "images/product");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                product.ImageUrl = fileName;
            }

            _unitOfWork.Product.Insert(product);
            _unitOfWork.Save();

            SuccessMessage = "New product added";
            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoryId"] = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name", product.CategoryId);
        ViewData["CoverTypeId"] = new SelectList(_unitOfWork.CoverType.GetAll(), "Id", "Name", product.CoverTypeId);
        return View("create", product);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _unitOfWork.Product.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewData["CategoryId"] = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name", product.CategoryId);
        ViewData["CoverTypeId"] = new SelectList(_unitOfWork.CoverType.GetAll(), "Id", "Name", product.CoverTypeId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id,
        [Bind("Id,Name,Author,ISBN,Price,Price50,Price100,Description,CategoryId,CoverTypeId")]
        Product product,
        IFormFile? file)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwRootPath, "images/product");

                //Delete old image
                var oldImagePath = Path.Combine(productPath, product.ImageUrl);
                if (System.IO.File.Exists(oldImagePath) && product.ImageUrl != "default.jpg")
                {
                    System.IO.File.Delete(oldImagePath);
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                product.ImageUrl = fileName;
            }

            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            SuccessMessage = "Product updated";
            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoryId"] = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name", product.CategoryId);
        ViewData["CoverTypeId"] = new SelectList(_unitOfWork.CoverType.GetAll(), "Id", "Name", product.CoverTypeId);
        return View("edit", product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, int? page)
    {
        var product = _unitOfWork.Product.GetById(id);
        _unitOfWork.Product.Delete(id);
        if (_unitOfWork.Save() > 0)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var productPath = Path.Combine(wwwRootPath, "images/product");

            var oldImagePath = Path.Combine(productPath, product!.ImageUrl);
            if (System.IO.File.Exists(oldImagePath) && product.ImageUrl != "default.jpg")
            {
                System.IO.File.Delete(oldImagePath);
            }
            SuccessMessage = "Product deleted";
        }

        var products = _unitOfWork.Product.GetAll();
        var pageNumber = page ?? 1;
        if (!Pager.HasProductsOnPage(products, pageNumber))
        {
            pageNumber -= 1;
        }

        return RedirectToAction(nameof(Index), new { page = pageNumber });
    }
}