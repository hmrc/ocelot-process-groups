using Microsoft.EntityFrameworkCore;

namespace ProductGrouping.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
        : base(options)
        { }

        public DbSet<ProductGroup> ProductGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>()
                        .HasIndex(p => p.ProductReference)
                        .IsUnique();
        }
    }
}
