using DataLayer.Models;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class DataContext : DbContext
{
    public DbSet<DbFuture> Futures { get; set; }
    public DbSet<DbOption> Options { get; set; }
    public DbSet<Container> Containers { get; set; }
    public DbSet<LongStraddle> Straddles { get; set; }
    public DbSet<OptionStrategy> OptionStrategies { get; set; }
    public DbSet<DbOrder> Orders { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbOption>()
            .HasMany(option => option.OptionStrategies)
            .WithOne(optionStrategy => optionStrategy.Option)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<DbFuture>()
            .Property(f => f.MinTick)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<DbOption>()
            .Property(f => f.MinTick)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<DbOption>()
            .Property(f => f.Strike)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<DbOrder>()
            .Property(o => o.Commission)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<DbOrder>()
            .Property(o => o.AvgFilledPrice)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<DbOrder>()
            .Property(o => o.LmtPrice)
            .HasColumnType("decimal(18,10)");

        modelBuilder.Entity<Container>()
            .Property(c => c.WantedPnl)
            .HasColumnType("decimal(18,2)");
    }
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlite($"Data Source={DbPath}");
}