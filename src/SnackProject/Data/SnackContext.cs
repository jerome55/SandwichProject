using Microsoft.EntityFrameworkCore;
using SnackProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Data
{
    public class SnackContext : DbContext {
        public SnackContext(DbContextOptions<SnackContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLineVegetable>()
                .HasKey(c => new { c.orderLineId, c.vegetableId });
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Sandwich> Sandwiches { get; set; }
        public DbSet<Vegetable> Vegetables { get; set; }
        public DbSet<Menu> Menus { get; set; }

    }
}
