using DataLayer.Models;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace DataLayer;

public class MFDllContext : DbContext
{
    public DbSet<Container> MainStrategies { get; set; }
    public DbSet<LongStraddle> LongStraddles { get; set; }
    public DbSet<OptionStrategy> OptionStrategies { get; set; }
    public DbSet<DbOrder> DbOrders { get; set; }
    public string DbPath { get; }
    public MFDllContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "MFDll.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}