using Microsoft.EntityFrameworkCore;
using MVC.Data;

namespace MVC.Dao;

public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public void Update(T obj)
        {
            _dbSet.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {

            T? existing = _dbSet.Find(id);
            if (existing != null)
            {
                _dbSet.Remove(existing);
            }
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }
    }