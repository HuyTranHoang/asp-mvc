using Microsoft.AspNetCore.Mvc;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;

namespace MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [TempData] public string? SuccessMessage { get; set; }

    public IActionResult Index(int? page, string? s)
    {
        var query = _unitOfWork.CoverType.GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(s))
        {
            s = s.Trim();
            query = query.Where(c => c.Name.Contains(s, StringComparison.OrdinalIgnoreCase));
        }

        var pageNumber = page ?? 1;
        var recsCount = query.Count();
        var pager = new Pager(recsCount, pageNumber);

        var recSkip = (pageNumber - 1) * pager.PageSize;

        var onePageOfCategoriesCoverTypes = query.Skip(recSkip).Take(pager.PageSize);

        ViewBag.Pager = pager;

        return View(onePageOfCategoriesCoverTypes);
    }

    public IActionResult Upsert(int? id)
    {
        CoverType coverType;

        if (id == null || id == 0)
        {
            coverType = new CoverType();
        }
        else
        {
            coverType = _unitOfWork.CoverType.GetById(id);
            if (coverType == null) return NotFound();
        }

        return View(coverType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(int id, [Bind("Id,Name")] CoverType coverType)
    {
        if (id != coverType.Id) return NotFound();

        if (!ModelState.IsValid) return View("upsert", coverType);

        if (id == 0)
        {
            _unitOfWork.CoverType.Insert(coverType);
            SuccessMessage = "New cover type added";
        }
        else
        {
            _unitOfWork.CoverType.Update(coverType);
            SuccessMessage = "Product updated";
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, int? page)
    {
        _unitOfWork.CoverType.Delete(id);
        if (_unitOfWork.Save() > 0) SuccessMessage = "Category deleted";

        var coverTypes = _unitOfWork.CoverType.GetAll();
        var pageNumber = page ?? 1;
        if (!Pager.HasProductsOnPage(coverTypes, pageNumber)) pageNumber -= 1;

        return RedirectToAction(nameof(Index), new { page = pageNumber });
    }
}