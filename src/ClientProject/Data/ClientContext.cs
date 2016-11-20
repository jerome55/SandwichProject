using Microsoft.EntityFrameworkCore;
using ClientProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Data
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options)
        {
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
        
    }
}
