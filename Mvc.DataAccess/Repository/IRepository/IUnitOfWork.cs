using MVC.Models;

namespace MVC.DataAccess.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    GenericRepository<Category> CategoryRepository { get; }
    int Save();
}