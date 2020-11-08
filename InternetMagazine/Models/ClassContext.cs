using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InternetMagazine.Models
{
    public class ClassContext : DbContext
    {
        public ClassContext(DbContextOptions<ClassContext> options) : base(options) { }

        public DbSet<Basket> Basket { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }      
        public DbSet<Review> Review { get; set; }
        public DbSet<WriteOfOrder> WriteOfOrder { get; set; }
    }
}
