using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;

namespace MVC.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private GenericRepository<Category>? _categoryRepository;

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

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }
}