using Microsoft.AspNetCore.Mvc;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities.Helpers;

namespace MVC.Areas.Customer.Controllers;

[Area("Customer")]
public class BasketController : Controller
{
    private readonly IBasketService _basketService;
    private readonly string shoppingCartSession = "_ShoppingCartSession";

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public IActionResult Index()
    {
        List<BasketItem> shoppingCartList;

        if(HttpContext.Session.Get<List<BasketItem>>(shoppingCartSession) != default)
        {
            shoppingCartList = HttpContext.Session.Get<List<BasketItem>>(shoppingCartSession)!;
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
