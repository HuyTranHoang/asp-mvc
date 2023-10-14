using MVC.Models;

namespace MVC.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    GenericRepository<Category> Category { get; }
    GenericRepository<Product> Product { get; }
    int Save();
}