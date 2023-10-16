using Microsoft.AspNetCore.Identity;
using Mvc.DataAccess.Data;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;

namespace Mvc.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private GenericRepository<Category>? _category;
    private GenericRepository<Product>? _product;
    private GenericRepository<ShoppingCart>? _shoppingCart;
    private GenericRepository<IdentityUser>? _identityUser;
    private GenericRepository<CoverType>? _coverType;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GenericRepository<Category> Category
    {
        get
        {
            if (_category == null)
            {
                _category = new GenericRepository<Category>(_dbContext);
            }

            return _category;
        }
    }

    public GenericRepository<Product> Product
    {
        get
        {
            if (_product == null)
            {
                _product = new GenericRepository<Product>(_dbContext);
            }

            return _product;
        }
    }

    public GenericRepository<ShoppingCart> ShoppingCart
    {
        get
        {
            if (_shoppingCart == null)
            {
                _shoppingCart = new GenericRepository<ShoppingCart>(_dbContext);
            }

            return _shoppingCart;
        }
    }

    public GenericRepository<IdentityUser> IdentityUser
    {
        get
        {
            if (_identityUser == null)
            {
                _identityUser = new GenericRepository<IdentityUser>(_dbContext);
            }

            return _identityUser;
        }
    }

    public GenericRepository<CoverType> CoverType
    {
        get
        {
            if (_coverType == null)
            {
                _coverType = new GenericRepository<CoverType>(_dbContext);
            }

            return _coverType;
        }
    }


    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }
}