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
            //orders = orders.Where(e => e.DateOfDelivery == DateTime.Today);

            Decimal total = 0;

            if (!String.IsNullOrEmpty(companyFilter))
            {
                orders = orders.Where(e => e.Company.Id.ToString().Equals(companyFilter));
                total = orders.First().TotalAmount;
            }

            IQueryable<OrderLine> orderlines = reduceOrder(orders.ToList()).AsQueryable();

            if (!String.IsNullOrEmpty(sandwichFilter))
            {
                orderlines = orderlines.Where(e => e.Sandwich.Id.ToString().Equals(sandwichFilter));
            }

            OrderViewModel model = new OrderViewModel { OrderLines = orderlines.ToList(), Sandwiches = _context.Sandwiches.OrderByDescending(x => x.Name).ToList(), Companies = _context.Companies.Where(e => e.Orders.Any()).OrderByDescending(e => e.Name).ToList(), Total = total};

            return View(model);
        }

        //Amélioration possible avec les méthodes Enumerable.Distinct() et Enumerable.Zip()
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

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOfDelivery,TotalAmount")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOfDelivery,TotalAmount")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
