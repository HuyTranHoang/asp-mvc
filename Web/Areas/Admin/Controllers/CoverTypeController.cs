using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc.DataAccess.Data;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [TempData] public string? SuccessMessage { get; set; }

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name")] CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Insert(coverType);
                _unitOfWork.Save();

                SuccessMessage = "New cover type added";
                return RedirectToAction(nameof(Index));
            }

            return View("create");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coverType = _unitOfWork.CoverType.GetById(id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, [Bind("Id,Name")] CoverType coverType)
        {
            if (id != coverType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();
                SuccessMessage = "Cover type updated";
                return RedirectToAction(nameof(Index));
            }

            return View("edit", coverType);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int? page)
        {
            _unitOfWork.CoverType.Delete(id);
            if (_unitOfWork.Save() > 0)
            {
                SuccessMessage = "Category deleted";
            }

            var coverTypes = _unitOfWork.CoverType.GetAll();
            var pageNumber = page ?? 1;
            if (!Pager.HasProductsOnPage(coverTypes, pageNumber))
            {
                pageNumber -= 1;
            }

            return RedirectToAction(nameof(Index), new { page = pageNumber });
        }

    }
}
