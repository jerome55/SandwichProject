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

        // GET: Sandwich
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sandwiches.ToListAsync());
        }

        // GET: Sandwich/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.Sandwiches.SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("id,available,description,name,price")] Sandwich sandwich)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sandwich);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sandwich);
        }

        // GET: Sandwich/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.Sandwiches.SingleOrDefaultAsync(m => m.Id == id);
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
        //public async Task<IActionResult> Edit(int id, [Bind("id,available,description,name,price")] Sandwich sandwich)
        public IActionResult Edit(int id, [Bind("id,available,description,name,price")] Sandwich sandwich)
        {
            if (id != sandwich.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UpdateSandwichTask newUpdateSandwichTask = new UpdateSandwichTask(sandwich);
                TenHourExecutionManager.AddNewTask(newUpdateSandwichTask);

                //CE CODE SE SITUE MAINTENANT DANS UpdateSandwichTask
                /*try
                {
                    _context.Update(sandwich);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SandwichExists(sandwich.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }*/
                return RedirectToAction("Index");
            }
            return View(sandwich);
        }

        /*
         * ON NE SUPPRIME JAMAIS LES SANDWICHES
         * 
         * // GET: Sandwich/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.Sandwiches.SingleOrDefaultAsync(m => m.id == id);
            if (sandwich == null)
            {
                return NotFound();
            }

            return View(sandwich);
        }

        // POST: Sandwich/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sandwich = await _context.Sandwiches.SingleOrDefaultAsync(m => m.id == id);
            _context.Sandwiches.Remove(sandwich);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SandwichExists(int id)
        {
            return _context.Sandwiches.Any(e => e.id == id);
        }*/
    }
}
