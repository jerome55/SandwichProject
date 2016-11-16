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
        public DbSet<Sandwich> sandwiches { get; set; }
        //public DbSet<Company> company { get; set; }
        public DbSet<Crudity> vegetables { get; set; }
        //public DbSet<Menu> menu { get; set; }
        //public DbSet<Order> orders { get; set; }
        //public DbSet<OrderLine> orderLines { get; set; }
    }
}
