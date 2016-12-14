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
using ClientProject.Models.Communication;
using ClientProject.Models.MenuViewModels;

namespace ClientProject.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Employee> _userManager;

        public MenuController(ApplicationDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Order GetCurrentOrder(Employee employee)
        {


            DateTime now = DateTime.Now;
            DateTime delivreryDate;
            if (now.Hour >= 10)
            {
                delivreryDate = DateTime.Today.AddDays(1.0);
            }
            else{
                delivreryDate = DateTime.Today;
            }

            List<Order> order = employee.Orders.Where(o => o.DateOfDelivery.Equals(delivreryDate)).ToList();
            //List<Order> order = _context.Orders.Where(o => o.DateOfDelivery.Equals(delivreryDate)).ToList();//o => o.Employee == employee && 

            if (order.Count == 0)
            {
                return null;
            }
            else
            {
                return order.First();
            }
        }


        // GET: Menu
        public async Task<IActionResult> Index()
        {
            RemoteCall remoteCaller = RemoteCall.GetInstance();
            CommWrap<Menu> responseMenu = await remoteCaller.GetMenu();

            MenuViewModel menuViewModel = new MenuViewModel();

            if (responseMenu.RequestStatus == 1)
            {
                Menu menu = responseMenu.Content;

                menuViewModel = new MenuViewModel { Menu = menu, SelectedSandwich = null };
            }
            AddErrors("Le menu n'a pu être chargé correctement");

            return View(menuViewModel);

            /*string id = _userManager.GetUserId(User);

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                                .Include(emp => emp.Orders)
                                    .ThenInclude(order => order.OrderLines)
                                        .ThenInclude(orderLine => orderLine.Sandwich)
                                .Include(emp => emp.Orders)
                                    .ThenInclude(order => order.OrderLines)
                                        .ThenInclude(orderLine => orderLine.OrderLineVegetables)
                                            .ThenInclude(orderLineVegetable => orderLineVegetable.Vegetable)
                                .SingleOrDefaultAsync(m => m.Id == id);

            */

            //string id = _userManager.GetUserId(User);

            //Employee emp = _context.Employees.Where(e => e.Id == id).FirstOrDefault();

            //Order order = GetCurrentOrder(emp);

            //string serializable = HttpContext.Session.GetString("cart");

            //Order order = JsonConvert.DeserializeObject<Order>(serializable);

            //return View(order.OrderLines.ToList());




            /*Menu menu = await RemoteCall.GetInstance().GetMenu();

            if(menu == null)
            {
                return View(menu);
            }
            else
            {
                return NotFound();
            }*/
        }



        // POST: OrderLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Index(MenuViewModel model)
        {
            if (ModelState.IsValid)
            {
            }
            return model.SelectedSandwich;
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


        private void AddErrors(String errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
        }
    }
}
