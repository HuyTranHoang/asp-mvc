using Microsoft.AspNetCore.Http;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;
using Mvc.Utilities.Helpers;

namespace Mvc.DataAccess.Repository;

public class BasketService : IBasketService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly string shoppingCartSession = "_ShoppingCartSession";

    public BasketService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public void Add(int id, int quantity)
    {
        List<BasketItem> shoppingCartList;
        if (_contextAccessor.HttpContext.Session.Get<List<BasketItem>>(shoppingCartSession) != default)
        {
            shoppingCartList = _contextAccessor.HttpContext.Session.Get<BasketItem>(shoppingCartSession);

            if(shoppingCartList.Where(i => i.Product.Id == 0).Any())
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
        _contextAccessor.HttpContext.Session.Set<List<BasketItem>>(shoppingCartSession, shoppingCartList);
    }

    public void Remove(int id)
    {
        if (_contextAccessor.HttpContext.Session.Get<List<BasketItem>>(shoppingCartSession) != default)
        {
            List<BasketItem> shoppingCartList = _contextAccessor.HttpContext.Session.Get<BasketItem>(shoppingCartSession);

            shoppingCartList.RemoveAll(i => i.Product.Id == id);

            _contextAccessor.HttpContext.Session.Set<List<BasketItem>>(shoppingCartSession, shoppingCartList);
        }
    }

    public void ClearBasket()
    {
        _contextAccessor.HttpContext.Session.Remove(shoppingCartSession);
    }
}