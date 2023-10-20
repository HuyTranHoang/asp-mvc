using System.Linq.Expressions;

namespace Mvc.DataAccess.Repository.IRepository;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetAll(string? includeProperties = null);
    // IEnumerable<T> Get(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    // public IEnumerable<T> Get(Expression<Func<T, bool>>? filter, string? includeProperties = null, string? orderBy = null, bool isDescending = false);
    public IEnumerable<T> Get(Expression<Func<T, bool>>? filter, string? includeProperties = null);


    T? GetById(object id);
    void Insert(T obj);
    void Update(T obj);
    void Delete(object id);
}