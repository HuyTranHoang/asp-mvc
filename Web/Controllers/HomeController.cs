using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repository.IRepository;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var products = _unitOfWork.Product.GetAll(includeProperties: "Category");
        return View(products);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "Category").FirstOrDefault();

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}