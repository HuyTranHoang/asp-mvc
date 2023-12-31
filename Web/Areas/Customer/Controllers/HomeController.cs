﻿using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities;

namespace MVC.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [TempData] public string? SuccessMessage { get; set; }


    public IActionResult Index()
    {
        var products = _unitOfWork.Product.GetAll("Category,CoverType");
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var basketItem = new BasketItem
        {
            Product = _unitOfWork.Product.Get(p => p.Id == id, "Category,CoverType").FirstOrDefault(),
            Count = 1
        };

        if (basketItem.Product == null) return NotFound();

        return View(basketItem);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Details([Bind("ProductId, Quantity")] ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        shoppingCart.IdentityUserId = userId;

        var cartFromDb = _unitOfWork.ShoppingCart
            .Get(u => u.IdentityUserId == userId && u.ProductId == shoppingCart.ProductId)
            .FirstOrDefault();

        if (cartFromDb != null)
        {
            cartFromDb.Quantity += shoppingCart.Quantity;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }
        else
        {
            _unitOfWork.ShoppingCart.Insert(shoppingCart);
        }

        _unitOfWork.Save();
        HttpContext.Session.SetInt32(SD.SessionCart,
            _unitOfWork.ShoppingCart.Get(u => u.IdentityUserId == userId).Count()
        );
        SuccessMessage = "Add to cart successfully!";
        return RedirectToAction(nameof(Index));
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