using System.Linq.Expressions;

namespace MVC.DataAccess.Repository.IRepository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    IEnumerable<T> Get(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy);
    T? GetById(object id);
    void Insert(T obj);
    void Update(T obj);
    void Delete(object id);
}