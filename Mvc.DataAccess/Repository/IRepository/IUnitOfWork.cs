using Microsoft.AspNetCore.Identity;
using Mvc.Models;

namespace Mvc.DataAccess.Repository.IRepository;

public interface IUnitOfWork : IDisposable
{
    GenericRepository<Category> Category { get; }
    GenericRepository<Product> Product { get; }
    GenericRepository<ShoppingCart> ShoppingCart { get; }
    GenericRepository<IdentityUser> IdentityUser { get; }
    int Save();
}