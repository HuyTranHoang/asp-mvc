using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; } = default!;
}