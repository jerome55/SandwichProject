using ClientProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            InitializeOrders(context);
        }
        private static void InitializeOrders(ApplicationDbContext context)
        {
            if (context.Orders.Any())
            {
                return;
            }
            if (context.Employees.Any())
            {
                Employee emp = context.Employees.First();
                Sandwich san = new Sandwich { Id = 1 };
                OrderLine ordLin = new OrderLine { Quantity = 1, Sandwich = san };
                List<OrderLine> ordLinList = new List<OrderLine>();
                ordLinList.Add(ordLin);
                Order ord = new Order { DateOfDelivery = DateTime.Now, TotalAmount = (decimal)1.00, OrderLines = ordLinList };
                List < Order > ordList= new List<Order>();
                ordList.Add(ord);
                emp.Orders = ordList;
                //context.Orders.Add(ord);
                context.SaveChanges();
            }
            
            
        }
    }
}