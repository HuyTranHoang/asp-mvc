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

    public IActionResult Index()
    {
        var coverTypes = _unitOfWork.CoverType.GetAll();
        return View(coverTypes);
    }

    public IActionResult Upsert(int? id)
    {
        CoverType coverType;

        if (id is null or 0)
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



    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var coverTypeList = _unitOfWork.CoverType.GetAll();
        return Json(new { data = coverTypeList });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _unitOfWork.CoverType.Delete(id);
        return Json(_unitOfWork.Save() > 0 ? new { success = true, message = "Cover type deleted" } : new { success = false, message = "Error while deleting" });
    }

    #endregion
}