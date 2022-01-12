using Microsoft.EntityFrameworkCore;

namespace ConsoleUI.Models
{
    internal class ReceiversContext : DbContext
    {
        public DbSet<Receiver>? Receivers { get; set; }

        public string DbPath { get; private set; }

        public ReceiversContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{Path.DirectorySeparatorChar}Receivers.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
