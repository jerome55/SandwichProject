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
    public class CrudityController : Controller
    {
        private readonly SnackContext _context;

        public CrudityController(SnackContext context)
        {
            _context = context;    
        }

        // GET: Crudity
        public async Task<IActionResult> Index()
        {
            return View(await _context.vegetables.ToListAsync());
        }

        // GET: Crudity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crudity = await _context.vegetables.SingleOrDefaultAsync(m => m.ID == id);
            if (crudity == null)
            {
                return NotFound();
            }

            return View(crudity);
        }

        // GET: Crudity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crudity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,available,description,name")] Crudity crudity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(crudity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(crudity);
        }

        // GET: Crudity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crudity = await _context.vegetables.SingleOrDefaultAsync(m => m.ID == id);
            if (crudity == null)
            {
                return NotFound();
            }
            return View(crudity);
        }

        // POST: Crudity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,available,description,name")] Crudity crudity)
        {
            if (id != crudity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crudity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrudityExists(crudity.ID))
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
            return View(crudity);
        }

        // GET: Crudity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crudity = await _context.vegetables.SingleOrDefaultAsync(m => m.ID == id);
            if (crudity == null)
            {
                return NotFound();
            }

            return View(crudity);
        }

        // POST: Crudity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crudity = await _context.vegetables.SingleOrDefaultAsync(m => m.ID == id);
            _context.vegetables.Remove(crudity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CrudityExists(int id)
        {
            return _context.vegetables.Any(e => e.ID == id);
        }
    }
}
