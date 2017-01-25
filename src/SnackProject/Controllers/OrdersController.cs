using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SnackProject.Data;
using SnackProject.Models;
using SnackProject.Models.OrderViewModels;

namespace SnackProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Orders
        [HttpGet]
        public async Task<IActionResult> Index(string companyFilter,string sandwichFilter)
        {
            ViewData["CompanyFilter"] = companyFilter;
            ViewData["SandwichFilter"] = sandwichFilter;

            var orders = _context.Orders.Include(u => u.OrderLines).ThenInclude(u => u.Sandwich).Include(u => u.OrderLines).ThenInclude(o => o.OrderLineVegetables).ThenInclude(u => u.Vegetable).AsQueryable();
            orders = orders.Where(e => e.DateOfDelivery == DateTime.Today);//filtre les commandes sur celles à livrer aujourd'hui

            Decimal total = 0;

            //filtre les commandes selon l'entreprise choisie
            if (!String.IsNullOrEmpty(companyFilter))
            {
                orders = orders.Where(e => e.Company.Id == Int32.Parse(companyFilter));
                total = orders.First().TotalAmount;
            }

            
            IQueryable<OrderLine> orderlines = reduceOrder(orders.ToList()).AsQueryable();

            //filtre les lignes de commandes selon le sandwich choisi
            if (!String.IsNullOrEmpty(sandwichFilter))
            {
                orderlines = orderlines.Where(e => e.Sandwich.Id.ToString().Equals(sandwichFilter));
            }

            //retourne un viewModel contenant la liste de lignes de commandes, le prix de la commande (=0 si le filtre entreprise n'est pas actif), la liste des entreprises ayant des commandes en cours, la liste des sandwiches.
            OrderViewModel model = new OrderViewModel { OrderLines = orderlines.ToList(), Sandwiches = _context.Sandwiches.OrderByDescending(x => x.Name).ToList(), Companies = _context.Companies.Where(e => e.Orders.Any()).OrderByDescending(e => e.Name).ToList(), Total = total};

            return View(model);
        }

        /* transforme la liste de commande en une liste de ligne de commande
         * cette liste est réduite pour su'une ligne de commande corresponde à un ensemble sandwich, liste de crudités unique ainsi qu'un nombre de sandwich
         * 
         * Amélioration possible avec les méthodes Enumerable.Distinct() et Enumerable.Zip()
        */
        private List<OrderLine> reduceOrder(List<Order> orders)
        {
            List<OrderLine> reduced = new List<OrderLine>();

            foreach (var order in orders)
            {
                foreach (var orderline in order.OrderLines)
                {
                    bool found = false;
                    for (int i = 0; i < reduced.Count && !found; i++)
                    {
                        if (reduced.ElementAt(i).Equals(orderline))
                        {
                            found = true;
                            reduced.ElementAt(i).Quantity += orderline.Quantity;
                        }
                    }
                    if (!found)
                    {
                        reduced.Add(orderline);
                    }
                }
            }

            return reduced;
        }
    }
}
