using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductGrouping.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
        : base(options)
        { }

        public DbSet<ProductGroup> ProductGroups { get; set; }
    }
}
