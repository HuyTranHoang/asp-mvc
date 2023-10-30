namespace Mvc.Models;

public class BasketItem
{
    public int Id { get; set; }

    public Product Product { get; set; }
    public int Count { get; set; }

}