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
            InitializeVegetable(context); 
        }

        private static void InitializeSandwich(SnackContext context) {
            if (context.Sandwiches.Any()) {
                return;
            }
            var sandwiches = new Sandwich[] {
                new Sandwich { name="Americain", description="...", price=(decimal)2.50, available=true },
                new Sandwich { name="Dagobert", description="...", price=(decimal)1.50, available=true }
            };
            foreach (Sandwich s in sandwiches) {
                context.Sandwiches.Add(s);
            }
            context.SaveChanges();
        }

        private static void InitializeVegetable(SnackContext context) {
            if (context.Vegetables.Any()) {
                return;
            }
            var vegetables = new Vegetable[] {
                new Vegetable{ name="Carotte", description="...", available=true },
                new Vegetable { name="Salade", description="...", available=true }
            };
            foreach (Vegetable s in vegetables) {
                context.Vegetables.Add(s);
            }
            context.SaveChanges();
        }
        
    }
}
