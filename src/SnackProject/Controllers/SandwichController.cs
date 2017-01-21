using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SnackProject.Data;
using SnackProject.Models;
using SnackProject.Automatics;

namespace SnackProject.Controllers
{
    public class SandwichController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SandwichController(ApplicationDbContext context)
        {
            _context = context;
        }


        /* Récupère et trie les sandwichs par odre de disponibilité
         * GET: Sandwich*/
        public IActionResult Index()
        {
            var i = TenHourExecutionManager.context.Sandwiches.OrderBy(x => x.Available ? 0 : 1);
            //Si le contexte contient des entrées ajoutées ou modifiées du type sandwich, récupère les sandwichs depuis le change tracker
            if (TenHourExecutionManager.context.ChangeTracker.Entries<Sandwich>().Any())
            {
                var j = TenHourExecutionManager.context.ChangeTracker.Entries<Sandwich>().Select(x => x.Entity as Sandwich).AsQueryable();
                i = j.OrderBy(x => x.Available ? 0 : 1);
            }
            return View(i);
        }


        // GET: Sandwich/Details/5
        // pas utilisé dans notre projet
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = TenHourExecutionManager.context.Sandwiches.SingleOrDefault(m => m.Id == id);
            if (TenHourExecutionManager.context.ChangeTracker.Entries<Sandwich>().Any())
            {
                sandwich = TenHourExecutionManager.context.ChangeTracker.Entries<Sandwich>().Select(x => x.Entity as Sandwich).AsQueryable().SingleOrDefault(m => m.Id == id);
            }
            if (sandwich == null)
            {
                return NotFound();
            }

            return View(sandwich);
        }

        // GET: Sandwich/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sandwich/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Available,Description,Name,Price")] Sandwich sandwich)
        {
            if (ModelState.IsValid)
            {
                sandwich.Available = true;
                TenHourExecutionManager.context.Entry(sandwich).State = EntityState.Added;
                return RedirectToAction("Index");
            }
            return View(sandwich);
        }

        // GET: Sandwich/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = TenHourExecutionManager.context.ChangeTracker.Entries<Sandwich>().Select(x => x.Entity as Sandwich).AsQueryable().SingleOrDefault(m => m.Id == id);

            if (sandwich == null)
            {
                return NotFound();
            }
            return View(sandwich);
        }

        // POST: Sandwich/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, [Bind("id,available,description,name,price")] Sandwich sandwich)
        public IActionResult Edit(int id, [Bind("Id,Available,Description,Name,Price")] Sandwich sandwich)
        {
            if (id != sandwich.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var oldEntity = TenHourExecutionManager.context.ChangeTracker.Entries<Sandwich>().Select(x => x.Entity as Sandwich).AsQueryable().SingleOrDefault(m => m.Id == id);
                if(TenHourExecutionManager.context.ChangeTracker.Entries().LastOrDefault().Context.Entry(oldEntity).State == EntityState.Added)
                {
                    oldEntity.Available = sandwich.Available;
                    oldEntity.Description = sandwich.Description;
                    oldEntity.Name = sandwich.Name;
                    oldEntity.Price = sandwich.Price;
                }
                else
                {
                    TenHourExecutionManager.context.Entry(oldEntity).State = EntityState.Detached;
                    TenHourExecutionManager.context.Entry(sandwich).State = EntityState.Modified;
                }
                
                return RedirectToAction("Index");
            }
            return View(sandwich);
        }

        //on ne supprime jamais les sandwichs
    }
}
