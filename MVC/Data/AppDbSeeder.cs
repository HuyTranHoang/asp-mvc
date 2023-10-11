using MVC.Models;

namespace MVC.Data;

public class AppDbSeeder
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

        if (context == null) return;

        context.Database.EnsureCreated();

        if (context.Categories.Any()) return;

        var categories = new List<Category>
        {
            new() { Name = "Manga", DisplayOrder = 1 },
            new() { Name = "Romance", DisplayOrder = 2 },
            new() { Name = "Fiction", DisplayOrder = 3 },
            new() { Name = "Programming", DisplayOrder = 4 }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();
    }
}