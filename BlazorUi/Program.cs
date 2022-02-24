using BlazorUi.Data;
using BlazorUi.Services;
using Connectors.IB;
using Connectors.Interfaces;
using DataLayer;
using DataLayer.Models.Instruments;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IConnector<DbFuture, DbOption>, IBConnector<DbFuture, DbOption>>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b =>
    {
        b.MigrationsAssembly("BlazorUi");
    });
});
builder.Services.AddScoped<FutureRepository>();
builder.Services.AddSingleton<TraderWorker>();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
