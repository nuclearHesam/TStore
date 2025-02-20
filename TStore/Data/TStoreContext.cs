using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace TStore.Data
{
    public class TStoreContext : IdentityDbContext<ApplicationUser>
    {
        public TStoreContext(DbContextOptions<TStoreContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
