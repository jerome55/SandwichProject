using SnackProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Data
{
    public class DbInitializer {

        public static void Initialize(ApplicationDbContext context) {
            context.Database.EnsureCreated();
            InitializeVegetablePrice(context);
            InitializeSandwich(context);
            InitializeVegetable(context); 
        }

        private static void InitializeVegetablePrice(ApplicationDbContext context)
        {
            if (context.Menus.Any())
            {
                return;
            }
            var menu = new Menu { VegetablesPrice = (decimal)0.50 };
            context.Menus.Add(menu);
            context.SaveChanges();
        }

        private static void InitializeSandwich(ApplicationDbContext context) {
            if (context.Sandwiches.Any()) {
                return;
            }
            var sandwiches = new Sandwich[] {
                new Sandwich { Name="Americain", Description="...", Price=(decimal)2.50, Available=true },
                new Sandwich { Name="Dagobert", Description="...", Price=(decimal)1.50, Available=true }
            };
            foreach (Sandwich s in sandwiches) {
                context.Sandwiches.Add(s);
            }
            context.SaveChanges();
        }

        private static void InitializeVegetable(ApplicationDbContext context) {
            if (context.Vegetables.Any()) {
                return;
            }
            var vegetables = new Vegetable[] {
                new Vegetable{ Name="Carotte", Description="...", Available=true },
                new Vegetable { Name="Salade", Description="...", Available=true }
            };
            foreach (Vegetable s in vegetables) {
                context.Vegetables.Add(s);
            }
            context.SaveChanges();
        }
        
    }
}
