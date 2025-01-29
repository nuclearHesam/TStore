using Microsoft.EntityFrameworkCore;

namespace TStore.Context;

public class TStoreContext : DbContext
{
    public DbSet<Category>? Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=TStore.db");
    }
}
