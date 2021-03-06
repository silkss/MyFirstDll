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

#region Singletons
builder.Services.AddSingleton<IConnector<DbFuture, DbOption>, IBConnector<DbFuture, DbOption>>();
builder.Services.AddSingleton<TraderWorker>();
builder.Services.AddSingleton<WeatherForecastService>();
#endregion

#region DB Context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b =>
    {
        b.MigrationsAssembly("BlazorUi");
    });
});
#endregion

#region Scoped
builder.Services.AddScoped<FutureRepository>();
builder.Services.AddScoped<ContainersRepository>();
builder.Services.AddScoped<OptionRepository>();
#endregion

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
app.MapGet("/api/mcapi", (string symbol, double price, string type, TraderWorker worker) =>
{
    if (type == "OPEN")
        worker.SignalOnOpen(symbol, price);
    else if (type == "CLOSE")
        worker.SignalOnClos(symbol, price);
});

app.Run();
