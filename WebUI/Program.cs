global using Microsoft.EntityFrameworkCore;
global using DataLayer;
global using DataLayer.Models.Instruments;
global using WebUi.Services;

using Connectors.IB;
using Connectors.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IConnector<DbFuture, DbOption>, IBConnector<DbFuture, DbOption>>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b =>
    {
        b.MigrationsAssembly("WebUi");
    });
});


var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
