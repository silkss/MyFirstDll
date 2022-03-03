using BlazorUi.Data;
using BlazorUi.Services;
using Connectors.IB;
using Connectors.Interfaces;
using DataLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

#region Singletons
builder.Services.AddSingleton<IConnector, IBConnector>();
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
builder.Services.AddScoped<OptionRepository>();
builder.Services.AddScoped<ContainersRepository>();
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
app.MapGet("/api/mcapi", async (string symbol, double price, string account, string type, 
    TraderWorker worker, ContainersRepository containersRepository,
    OptionRepository optionRepository) =>
{
    if (type == "OPEN")
        await worker.SignalOnOpenAsync(symbol, price, account, containersRepository, optionRepository);
    else if (type == "CLOSE")
        worker.SignalOnClose(symbol, price);
});

app.Run();
