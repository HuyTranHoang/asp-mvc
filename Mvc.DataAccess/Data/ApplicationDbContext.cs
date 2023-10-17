using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mvc.Models;

namespace Mvc.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<CoverType> CoverTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Manga", DisplayOrder = 1, CreatedAt = new DateTime(2023, 10, 17, 2, 40, 50)},
            new Category { Id = 2, Name = "Romance", DisplayOrder = 2, CreatedAt = new DateTime(2023, 10, 17, 3, 44, 12) },
            new Category { Id = 3, Name = "Fiction", DisplayOrder = 3, CreatedAt = new DateTime(2023, 10, 17, 4, 55, 23) },
            new Category { Id = 4, Name = "Programming", DisplayOrder = 4, CreatedAt = new DateTime(2023, 10, 17, 5, 22, 34) });

        builder.Entity<CoverType>().HasData(
            new CoverType { Id = 1, Name = "Softcover", CreatedAt = new DateTime(2023, 10, 17, 2, 40, 50)},
            new CoverType { Id = 2, Name = "Paperback", CreatedAt = new DateTime(2023, 10, 17, 3, 40, 50)},
            new CoverType { Id = 3, Name = "Hardcover", CreatedAt = new DateTime(2023, 10, 17, 4, 40, 50)});
    }
}

