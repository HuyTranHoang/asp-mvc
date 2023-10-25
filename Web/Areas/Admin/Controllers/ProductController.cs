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

    public IActionResult Index()
    {
        var products = _unitOfWork.Product.GetAll();

        return View(products);
    }

    public IActionResult Details(int? id)
    {
        if (id is null) return NotFound();

        var product = _unitOfWork.Product.GetById(id);

        if (product == null) return NotFound();

        return View(product);
    }


    public IActionResult Upsert(int? id)
    {
        var productDto = new ProductDto();

        if (id is null or 0)
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
    public IActionResult Upsert(int id, [Bind("Product")] ProductDto productDto, IFormFile? file)
    {
        if (id != productDto.Product.Id) return NotFound();
        var isCreate = productDto.Product.Id == 0;

        Validation(productDto.Product, file);

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

            if (isCreate)
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


    private void Validation(Product product, IFormFile? file)
    {
        var isCreate = product.Id == 0;

        var productNameExist = _unitOfWork.Product
            .Get(isCreate
                ? p => p.Name == product.Name
                : p => p.Name == product.Name && p.Id != product.Id)
            .Any();

        if (isCreate && file == null)
        {
            ModelState.AddModelError("Product.ImageUrl", "Product image is required");
        }

        if (productNameExist)
            ModelState.AddModelError("Name", "The category name already exist");
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = productList });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _unitOfWork.Product.Delete(id);
        return Json(_unitOfWork.Save() > 0 ? new { success = true, message = "Product deleted" } : new { success = false, message = "Error while deleting" });
    }

    #endregion
}