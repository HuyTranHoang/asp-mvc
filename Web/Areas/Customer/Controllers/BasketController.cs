using Microsoft.AspNetCore.Mvc;
using Mvc.Models;
using MVC.Services;
using Mvc.Utilities;

namespace MVC.Areas.Customer.Controllers;

[Area("Customer")]
public class BasketController : Controller
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public IActionResult Index()
    {
        List<BasketItem> shoppingCartList;

        if(HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession) != default)
        {
            shoppingCartList = HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession)!;
        } else
        {
            shoppingCartList = new List<BasketItem>();
        }

        return View(shoppingCartList);
    }

    public IActionResult AddToCart(int id)
    {
        _basketService.Add(id, 1);
        return RedirectToAction("Index", "Home");
    }
}
