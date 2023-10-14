using Mvc.DataAccess.Data;
using Mvc.DataAccess.Repository.IRepository;
using Mvc.Models;

namespace Mvc.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private GenericRepository<Category>? _category;
    private GenericRepository<Product>? _product;

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


    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }
}