namespace MVC.Services;

public interface IBasketService
{
    public void Add(int id, int quantity);
    public void Remove(int id);
    public void IncreaseQuantity(int id);
    public void DecreaseQuantity(int id);
    public void ClearBasket();

}