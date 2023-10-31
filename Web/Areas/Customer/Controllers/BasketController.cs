using Microsoft.AspNetCore.Mvc;
using Mvc.Models;
using MVC.Services;
using Mvc.Utilities;
using MVC.ViewModels;

namespace MVC.Areas.Customer.Controllers;

[Area("Customer")]
public class BasketController : Controller
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [TempData] public string? SuccessMessage { get; set; }

    public IActionResult Index()
    {
        ShoppingCartDto shoppingCartDto = new ShoppingCartDto();

        if(HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession) != default)
        {
            shoppingCartDto.BasketItemList = HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession)!;
        } else
        {
            shoppingCartDto.BasketItemList = new List<BasketItem>();
        }

        foreach (var cart in shoppingCartDto.BasketItemList)
        {
            cart.Price = GetPriceBasedOnquantity(cart);
            shoppingCartDto.OrderTotal += cart.Count * cart.Price;
        }

        return View(shoppingCartDto);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult AddToCart([Bind("Product, Count")] BasketItem basketItem)
    {
        _basketService.Add(basketItem.Product.Id, basketItem.Count);
        SuccessMessage = "Add to cart successfully!";
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Plus(int productId)
    {
        _basketService.IncreaseQuantity(productId);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int productId)
    {
        _basketService.DecreaseQuantity(productId);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int productId)
    {
        _basketService.Remove(productId);
        return RedirectToAction(nameof(Index));
    }

    private static double GetPriceBasedOnquantity(BasketItem basketItem)
    {
        if (basketItem.Count <= 50) return basketItem.Product.Price;

        if (basketItem.Count <= 100) return basketItem.Product.Price50 ?? basketItem.Product.Price;

        return basketItem.Product.Price100 ?? basketItem.Product.Price;
    }
}
