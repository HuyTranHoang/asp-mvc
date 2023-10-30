namespace Mvc.DataAccess.Repository.IRepository;

public interface IBasketService
{
    public void Add(int id, int quantity);
    public void Remove(int id);

}