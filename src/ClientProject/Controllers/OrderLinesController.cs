using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientProject.Data;
using ClientProject.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ClientProject.Controllers
{
    public class OrderLinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Employee> _userManager;

        public OrderLinesController(ApplicationDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Order getCurrentOrder(Employee employee)
        {


            DateTime now = DateTime.Now;
            DateTime delivreryDate;
            if (now.Hour >= 10)
            {
                delivreryDate = DateTime.Parse(now.Year+"-"+now.Month+"-"+now.Day+1);
            }
            else{
                delivreryDate = DateTime.Parse(now.Year + "-" + now.Month + "-" + now.Day);
            }

            List<Order> order = employee.Orders.Where(q => q.DateOfDelivery.Equals(delivreryDate)).ToList();

            if (order.Count == 0)
            {
                return null;
            }
            else
            {
                return order.First();
            }
        }


        // GET: OrderLines
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string id = _userManager.GetUserId(User);

            Employee emp = await _userManager.FindByIdAsync(id);

            //Order order = getCurrentOrder(emp);

            string serializable = HttpContext.Session.GetString("cart");

            Order order = JsonConvert.DeserializeObject<Order>(serializable);

            return View(order.OrderLines.ToList());
        }

        // GET: OrderLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines.SingleOrDefaultAsync(m => m.Id == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // GET: OrderLines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,quantity")] OrderLine orderLine)
        {
            if (ModelState.IsValid)
            {
                string orderLineSerializable = JsonConvert.SerializeObject(orderLine);
                HttpContext.Session.SetString("cart", orderLineSerializable);
                //_context.Add(orderLine);
                //await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(orderLine);
        }

        // GET: OrderLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines.SingleOrDefaultAsync(m => m.Id == id);
            if (orderLine == null)
            {
                return NotFound();
            }
            return View(orderLine);
        }

        // POST: OrderLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,quantity")] OrderLine orderLine)
        {
            if (id != orderLine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderLineExists(orderLine.Id))
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
            return View(orderLine);
        }

        // GET: OrderLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines.SingleOrDefaultAsync(m => m.Id == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // POST: OrderLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderLine = await _context.OrderLines.SingleOrDefaultAsync(m => m.Id == id);
            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrderLines.Any(e => e.Id == id);
        }
    }
}
