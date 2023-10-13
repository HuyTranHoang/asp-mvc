using System.Linq.Expressions;

namespace MVC.Repository.IRepository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetAll(string? includeProperties = null);
    // IEnumerable<T> Get(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    public IEnumerable<T> Get(Expression<Func<T, bool>>? filter, string? orderBy = null, bool isDescending = false);

    T? GetById(object id);
    void Insert(T obj);
    void Update(T obj);
    void Delete(object id);
}