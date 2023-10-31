using Mvc.Models;

namespace MVC.ViewModels;

public class ShoppingCartDto
{
    public IEnumerable<BasketItem> BasketItemList { get; set; }

    public double OrderTotal { get; set; }
}