using Microsoft.EntityFrameworkCore;

namespace ProductGrouping.Models
{
    /// <summary>
    /// Database context
    /// </summary>
    public class Context : DbContext
    {
        /// <summary>
        /// Context constructor
        /// </summary>
        /// <param name="options">Options</param>
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Product Groups
        /// </summary>
        public DbSet<ProductGroup> ProductGroups { get; set; }

        /// <summary>
        /// Runs on creation
        /// </summary>
        /// <param name="modelBuilder">Model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductGroup>()
                        .HasIndex(p => p.ProductReference)
                        .IsUnique();

            modelBuilder.Entity<ProductGroup>()
                        .Property(p => p.ProductOwner)
                        .IsFixedLength()
                        .IsUnicode(false);
        }
    }
}
