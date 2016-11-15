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
            InitializeSandwich(context);
            InitializeCrutity(context);
        }

        private static void InitializeSandwich(SnackContext context) {
            if (context.sandwiches.Any()) {
                return;
            }
            var sandwiches = new Sandwich[] {
                new Sandwich { name="Americain", description="...", price=2.5D, available=true },
                new Sandwich { name="Dagobert", description="...", price=1.5D, available=true }
            };
            foreach (Sandwich s in sandwiches) {
                context.sandwiches.Add(s);
            }
            context.SaveChanges();
        }

        private static void InitializeCrutity(SnackContext context) {
            if (context.vegetables.Any()) {
                return;
            }
            var vegetables = new Crudity[] {
                new Crudity{ name="Carotte", description="...", available=true },
                new Crudity { name="Salade", description="...", available=true }
            };
            foreach (Crudity s in vegetables) {
                context.vegetables.Add(s);
            }
            context.SaveChanges();
        }
    }
}
