using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities;
using MVC.ViewModels;

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

    [TempData] public string? SuccessMessage { get; set; }

    public IActionResult Index(int? page, string? s, string? sortOrder)
    {
        var query = _unitOfWork.Product
            .GetAll("Category")
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
        if (id == null) return NotFound();

        var product = _unitOfWork.Product.GetById(id);

        if (product == null) return NotFound();

        return View(product);
    }


    public IActionResult Upsert(int? id)
    {
        var productDto = new ProductDto();

        if (id == null || id == 0)
        {
            productDto.Product = new Product();
        }
        else
        {
            productDto.Product = _unitOfWork.Product.GetById(id);
            if ( productDto.Product == null) return NotFound();
        }

        productDto.CategoryList = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name",  productDto.Product.CategoryId);
        productDto.CoverTypeList = new SelectList(_unitOfWork.CoverType.GetAll(), "Id", "Name",  productDto.Product.CoverTypeId);
        return View(productDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(int id,
        [Bind("Product")]
        ProductDto productDto,
        IFormFile? file)
    {
        if (id != productDto.Product.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwRootPath, "images/product");

                //Delete old image
                var oldImagePath = Path.Combine(productPath, productDto.Product.ImageUrl);
                if (System.IO.File.Exists(oldImagePath) && productDto.Product.ImageUrl != "default.jpg")
                    System.IO.File.Delete(oldImagePath);

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productDto.Product.ImageUrl = fileName;
            }

            if (id == 0)
            {
                _unitOfWork.Product.Insert(productDto.Product);
                SuccessMessage = "New product added";
            }
            else
            {
                _unitOfWork.Product.Update(productDto.Product);
                SuccessMessage = "Product updated";
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        productDto.CategoryList = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name", productDto.Product.CategoryId);
        productDto.CoverTypeList = new SelectList(_unitOfWork.CoverType.GetAll(), "Id", "Name", productDto.Product.CoverTypeId);
        return View("upsert", productDto);
    }

    [HttpPost]
    [ActionName("Delete")]
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
                System.IO.File.Delete(oldImagePath);
            SuccessMessage = "Product deleted";
        }

        var products = _unitOfWork.Product.GetAll();
        var pageNumber = page ?? 1;
        if (!Pager.HasProductsOnPage(products, pageNumber)) pageNumber -= 1;

        return RedirectToAction(nameof(Index), new { page = pageNumber });
    }
}