using MVC.Models;

namespace MVC.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    GenericRepository<Category> CategoryRepository { get; }
    GenericRepository<Product> ProductRepository { get; }
    int Save();
}