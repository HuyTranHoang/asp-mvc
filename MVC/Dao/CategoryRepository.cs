using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;

namespace MVC.Dao;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Category> GetAll()
    {
        return _dbContext.Categories;
    }

    public Category? GetById(int id)
    {
        return _dbContext.Categories.Find(id);
    }

    public void Add(Category category)
    {
        _dbContext.Add(category);
    }

    public void Update(Category category)
    {
        // _dbContext.Update(category);
        _dbContext.Attach(category);
        _dbContext.Entry(category).State = EntityState.Modified;
    }

    public void Delete(Category category)
    {
        _dbContext.Remove(category);
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }
}