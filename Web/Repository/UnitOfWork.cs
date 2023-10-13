using MVC.Data;
using MVC.Models;
using MVC.Repository.IRepository;

namespace MVC.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private GenericRepository<Category>? _categoryRepository;
    private GenericRepository<Product>? _productRepository;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public GenericRepository<Category> CategoryRepository
    {
        get
        {
            if (_categoryRepository == null)
            {
                _categoryRepository = new GenericRepository<Category>(_dbContext);
            }
            return _categoryRepository;
        }
    }

    public GenericRepository<Product> ProductRepository
    {
        get
        {
            if (_productRepository == null)
            {
                _productRepository = new GenericRepository<Product>(_dbContext);
            }

            return _productRepository;
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