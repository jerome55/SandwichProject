using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientProject.Data;
using ClientProject.Models;

namespace ClientProject.Controllers
{
    public class VegetablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VegetablesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Vegetables
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vegetable.ToListAsync());
        }

        // GET: Vegetables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = await _context.Vegetable.SingleOrDefaultAsync(m => m.id == id);
            if (vegetable == null)
            {
                return NotFound();
            }

            return View(vegetable);
        }

        // GET: Vegetables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vegetables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id")] Vegetable vegetable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vegetable);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vegetable);
        }

        // GET: Vegetables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = await _context.Vegetable.SingleOrDefaultAsync(m => m.id == id);
            if (vegetable == null)
            {
                return NotFound();
            }
            return View(vegetable);
        }

        // POST: Vegetables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id")] Vegetable vegetable)
        {
            if (id != vegetable.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vegetable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VegetableExists(vegetable.id))
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
            return View(vegetable);
        }

        // GET: Vegetables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = await _context.Vegetable.SingleOrDefaultAsync(m => m.id == id);
            if (vegetable == null)
            {
                return NotFound();
            }

            return View(vegetable);
        }

        // POST: Vegetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vegetable = await _context.Vegetable.SingleOrDefaultAsync(m => m.id == id);
            _context.Vegetable.Remove(vegetable);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VegetableExists(int id)
        {
            return _context.Vegetable.Any(e => e.id == id);
        }
    }
}
