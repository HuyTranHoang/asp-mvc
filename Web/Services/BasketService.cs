using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities;

namespace MVC.Services;

public class BasketService : IBasketService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;


    public BasketService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public void Add(int id, int quantity)
    {
        List<BasketItem> shoppingCartList;
        if (_contextAccessor.HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession) != default)
        {
            shoppingCartList = _contextAccessor.HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession);

            if(shoppingCartList.Any(i => i.Product.Id == 0))
            {
                shoppingCartList.Where(i => i.Product.Id == id).Select(x =>
                {
                    x.Count += quantity;
                    return x;
                }).ToList();
            } else
            {
                shoppingCartList.Add(new BasketItem
                {
                    Count = quantity,
                    Product = _unitOfWork.Product.GetById(id)
                });
            }
        } else
        {
            shoppingCartList = new List<BasketItem>
            {
                new BasketItem
                {
                    Count = quantity,
                    Product = _unitOfWork.Product.GetById(id)
                }
            };
        }
        _contextAccessor.HttpContext.Session.Set<List<BasketItem>>(SD.ShoppingCartSession, shoppingCartList);
    }

    public void Remove(int id)
    {
        if (_contextAccessor.HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession) != default)
        {
            List<BasketItem> shoppingCartList = _contextAccessor.HttpContext.Session.Get<List<BasketItem>>(SD.ShoppingCartSession);

            shoppingCartList.RemoveAll(i => i.Product.Id == id);

            _contextAccessor.HttpContext.Session.Set<List<BasketItem>>(SD.ShoppingCartSession, shoppingCartList);
        }
    }

    public void ClearBasket()
    {
        _contextAccessor.HttpContext.Session.Remove(SD.ShoppingCartSession);
    }
}