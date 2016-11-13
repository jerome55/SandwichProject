using SnackProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Data
{
    public class DbInitializer {
        public static void Initialize(SnackContext context) {
            context.Database.EnsureCreated();

            if (context.sandwiches.Any()) {
                return;
            }
            var sandwiches = new Sandwich[] {
            new Sandwich { Name="Americain", Description="vla", Price=2.5D, Available=true },
            new Sandwich { Name="Dagobert", Description="desc", Price=1.5D, Available=true }
        };
            foreach (Sandwich s in sandwiches) {
                context.sandwiches.Add(s);
            }
            context.SaveChanges();
        }
    }
}
