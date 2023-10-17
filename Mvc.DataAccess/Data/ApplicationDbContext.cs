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
            new Category
                { Id = 1, Name = "Manga", DisplayOrder = 1, CreatedAt = new DateTime(2023, 10, 17, 2, 40, 50) },
            new Category
                { Id = 2, Name = "Romance", DisplayOrder = 2, CreatedAt = new DateTime(2023, 10, 17, 3, 44, 12) },
            new Category
                { Id = 3, Name = "Fiction", DisplayOrder = 3, CreatedAt = new DateTime(2023, 10, 17, 4, 55, 23) },
            new Category
                { Id = 4, Name = "Programming", DisplayOrder = 4, CreatedAt = new DateTime(2023, 10, 17, 5, 22, 34) });

        builder.Entity<CoverType>().HasData(
            new CoverType { Id = 1, Name = "Softcover", CreatedAt = new DateTime(2023, 10, 17, 2, 40, 50) },
            new CoverType { Id = 2, Name = "Paperback", CreatedAt = new DateTime(2023, 10, 17, 3, 40, 50) },
            new CoverType { Id = 3, Name = "Hardcover", CreatedAt = new DateTime(2023, 10, 17, 4, 40, 50) });

        builder.Entity<Product>().HasData(
            new Product
            {
                Id = 1, Name = "Secrets of Divine Love: A Spiritual Journey into the Heart of Islam",
                Author = "Neque porro",
                Price = 50000, Price50 = 40000, Price100 = 30000,
                Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. " +
                    "Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, " +
                    "ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. " +
                    "Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. " +
                    "Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. " +
                    "Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, " +
                    "vestibulum lobortis justo volutpat, vulputate vestibulum orci.",
                ISBN = "1234567890123",
                CategoryId = 1,
                CoverTypeId = 1,
                CreatedAt = new DateTime(2023, 10, 17, 7, 40, 50)
            },
            new Product
            {
                Id = 2, Name = "I Was Told There'd Be Cake: Essays",
                Author = "Sloane Crosley (Goodreads Author)",
                Price = 25000, Price50 = 15000, Price100 = 10000,
                Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. " +
                    "Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, " +
                    "ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. " +
                    "Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. " +
                    "Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. " +
                    "Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, " +
                    "vestibulum lobortis justo volutpat, vulputate vestibulum orci.",
                ISBN = "1234567890124",
                CategoryId = 2,
                CoverTypeId = 2,
                CreatedAt = new DateTime(2023, 10, 18, 7, 40, 50)
            },
            new Product
            {
                Id = 3, Name = "A Clockwork Orange",
                Author = "Anthony Burgess",
                Price = 40000, Price50 = 30000, Price100 = 20000,
                Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. " +
                    "Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, " +
                    "ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. " +
                    "Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. " +
                    "Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. " +
                    "Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, " +
                    "vestibulum lobortis justo volutpat, vulputate vestibulum orci.",
                ISBN = "1234567890125",
                CategoryId = 3,
                CoverTypeId = 3,
                CreatedAt = new DateTime(2023, 10, 19, 8, 40, 50)
            },
            new Product
            {
                Id = 4, Name = "Do Androids Dream of Electric Sheep?",
                Author = "Philip K. Dick",
                Price = 60000, Price50 = 40000, Price100 = 20000,
                Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam at erat vitae odio porttitor egestas. " +
                    "Duis tempor congue ex, luctus gravida felis ullamcorper nec. Donec mollis urna at justo pellentesque, " +
                    "ut sodales purus lobortis. Proin lacus tellus, lacinia eu leo ut, congue sollicitudin urna. In at varius lorem. " +
                    "Aliquam ornare eleifend dui, quis convallis orci luctus nec. Donec pellentesque molestie mi pretium accumsan. " +
                    "Phasellus rutrum, magna in finibus feugiat, sapien ligula varius odio, vel viverra velit diam id augue. " +
                    "Nunc felis mi, dignissim sit amet sollicitudin eu, tincidunt ac mi. Praesent eu pulvinar mi. Vestibulum leo ante, " +
                    "vestibulum lobortis justo volutpat, vulputate vestibulum orci.",
                ISBN = "1234567890126",
                CategoryId = 1,
                CoverTypeId = 2,
                CreatedAt = new DateTime(2023, 10, 20, 8, 40, 50)
            });
    }
}