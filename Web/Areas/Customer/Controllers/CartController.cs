using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities;
using MVC.ViewModels;

namespace MVC.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ShoppingCartVM ShoppingCartVM { get; set; } = new();

    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        ShoppingCartVM = new ShoppingCartVM
        {
            ShoppingCartList = _unitOfWork.ShoppingCart.Get(u => u.IdentityUserId == userId,
                includeProperties: "Product")
        };

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnquantity(cart);
            ShoppingCartVM.OrderTotal += cart.Quantity * cart.Price;
        }

        return View(ShoppingCartVM);
    }

    public IActionResult Plus(int cartId)
    {
        var cartFormDb = _unitOfWork.ShoppingCart.GetById(cartId);
        if (cartFormDb != null)
        {
            cartFormDb.Quantity += 1;
            _unitOfWork.ShoppingCart.Update(cartFormDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    public IActionResult Minus(int cartId)
    {
        var cartFormDb = _unitOfWork.ShoppingCart.GetById(cartId);
        if (cartFormDb != null)
        {
            cartFormDb.Quantity -= 1;
            _unitOfWork.ShoppingCart.Update(cartFormDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    public IActionResult Remove(int cartId)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        _unitOfWork.ShoppingCart.Delete(cartId);
        if (_unitOfWork.Save() > 0)
        {
            HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.Get(u => u.IdentityUserId == userId).Count());
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    private double GetPriceBasedOnquantity(ShoppingCart shoppingCart)
    {
        if (shoppingCart.Quantity <= 50)
        {
            return shoppingCart.Product.Price;
        }

        if (shoppingCart.Quantity <= 100)
        {
            return shoppingCart.Product.Price50 ?? shoppingCart.Product.Price;
        }

        return shoppingCart.Product.Price100 ?? shoppingCart.Product.Price;
    }
}