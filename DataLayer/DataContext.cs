using DataLayer.Models;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class DataContext : DbContext
{
    public DbSet<DbFuture> Futures { get; set; }
    public DbSet<Container> Containers { get; set; }
    public DbSet<LongStraddle> Straddles { get; set; }
    //public DbSet<OptionStrategy> OptionStrategies { get; set; }

    //public DbSet<LongStraddle> LongStraddles { get; set; }
    //public DbSet<OptionStrategy> OptionStrategies { get; set; }
    //public DbSet<DbOrder> DbOrders { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DbFuture>()
            .Property(f => f.MinTick)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<DbOption>()
            .Property(f => f.MinTick)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<DbOption>()
            .Property(f => f.Strike)
            .HasColumnType("decimal(18,10)");

    }
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlite($"Data Source={DbPath}");
}