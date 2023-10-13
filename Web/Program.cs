using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;
using MVC.Repository;
using MVC.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    var connectString = builder.Configuration.GetConnectionString("dbContext");
    options.UseSqlServer(connectString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// builder.Logging.ClearProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();