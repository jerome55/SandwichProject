using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientProject.Data;
using ClientProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using ClientProject.Models.Communication;
using Microsoft.AspNetCore.Authorization;

namespace ClientProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Employee> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        // GET: Employees
        [Authorize(Roles = "Employe, Responsable")]
        [Route("Profile/Index")]
        public async Task<IActionResult> Index()
        {
            string id = _userManager.GetUserId(User);

            if (id == null)
            {
                return NotFound();
            }

            //Permet de recuperer un employer ainsi que toutes c'est commandes avec toutes les info ( sandwich , cruditees). 
            Employee employee = await _context.Employees
                .Include(emp => emp.Orders)
                    .ThenInclude(order => order.OrderLines)
                    .ThenInclude(odLin => odLin.Sandwich)
                .Include(emp => emp.Orders)
                    .ThenInclude(order => order.OrderLines)
                    .ThenInclude(odLin => odLin.OrderLineVegetables)
                    .ThenInclude(odLinVeg => odLinVeg.Vegetable)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            //Permet de supprimer les commande dont l'on a pas besoin pour l'affichage des commande du jour ou du lendemain si apres  10h
            DateTime now = DateTime.Now;
            DateTime delivreryDate;
            if (now.Hour >= 10)
            {
                delivreryDate = DateTime.Today.AddDays(1.0);
            }
            else
            {
                delivreryDate = DateTime.Today;
            }
            employee.Orders = employee.Orders.Where(q => q.DateOfDelivery.Equals(delivreryDate)).ToList();

            //Permet de recupere les infos relatives aux sandwich et cruditées des commandes
            foreach (Order order in employee.Orders){
                foreach (OrderLine orderline in order.OrderLines)
                {
                   CommWrap<Sandwich> CommSan = await RemoteCall.GetInstance().GetSandwichById(orderline.Sandwich.Id);
                    if(CommSan.RequestStatus == 1)
                    {
                        orderline.Sandwich = CommSan.Content;
                    }else
                    {
                        return NotFound();
                    }
                    foreach(OrderLineVegetable orderLineVeg in orderline.OrderLineVegetables)
                    {
                        CommWrap<Vegetable> CommVeg = await RemoteCall.GetInstance().GetVegetableById(orderLineVeg.Vegetable.Id);
                        if (CommVeg.RequestStatus == 1)
                        {
                            orderLineVeg.Vegetable = CommVeg.Content;
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return View(employee);
        }

        /*
        // GET: Employees/Details/5s
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,FirstName,LastName,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,Wallet")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,FirstName,LastName,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,Wallet")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }*/

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
