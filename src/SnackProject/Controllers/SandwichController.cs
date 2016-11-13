using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SnackProject.Data;
using SnackProject.Models;

namespace SnackProject.Controllers
{
    public class SandwichController : Controller
    {
        private readonly SnackContext _context;

        public SandwichController(SnackContext context)
        {
            _context = context;    
        }

        // GET: Sandwich
        public async Task<IActionResult> Index()
        {
            return View(await _context.sandwiches.ToListAsync());
        }

        // GET: Sandwich/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.sandwiches.SingleOrDefaultAsync(m => m.ID == id);
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
        public async Task<IActionResult> Create([Bind("ID,Available,Description,Name,Price")] Sandwich sandwich)
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

            var sandwich = await _context.sandwiches.SingleOrDefaultAsync(m => m.ID == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Available,Description,Name,Price")] Sandwich sandwich)
        {
            if (id != sandwich.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sandwich);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SandwichExists(sandwich.ID))
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
            return View(sandwich);
        }

        // GET: Sandwich/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.sandwiches.SingleOrDefaultAsync(m => m.ID == id);
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
            var sandwich = await _context.sandwiches.SingleOrDefaultAsync(m => m.ID == id);
            _context.sandwiches.Remove(sandwich);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SandwichExists(int id)
        {
            return _context.sandwiches.Any(e => e.ID == id);
        }
    }
}
