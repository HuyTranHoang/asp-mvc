using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using MVC.Repository.IRepository;


namespace MVC.Controllers
{
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

        public IActionResult Index(int? page)
        {
            var query = _unitOfWork.ProductRepository
                .GetAll(includeProperties: "Category")
                .AsQueryable();

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

            var product = _unitOfWork.ProductRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Store([Bind("Name,Price,Description,CategoryId")] Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var productPath = Path.Combine(wwwRootPath, "images/product");


                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    product.ImageUrl = fileName;
                }

                _unitOfWork.ProductRepository.Insert(product);
                _unitOfWork.Save();

                SuccessMessage = "New product added";
                return RedirectToAction("Index");
            }

            ViewData["CategoryId"] = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View("create",product);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, [Bind("Id,Name,Price,Description,ImageUrl,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                SuccessMessage = "Product updated";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_unitOfWork.CategoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View("edit", product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, string? page)
        {
            _unitOfWork.ProductRepository.Delete(id);
            if (_unitOfWork.Save() > 0)
            {
                SuccessMessage = "Product deleted";
            }
            return RedirectToAction("Index", new { page });
        }

    }
}
