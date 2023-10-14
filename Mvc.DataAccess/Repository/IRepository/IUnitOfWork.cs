using MVC.Models;

namespace Mvc.DataAccess.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    GenericRepository<Category> Category { get; }
    GenericRepository<Product> Product { get; }
    int Save();
}