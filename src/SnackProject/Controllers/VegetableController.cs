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
    public class VegetableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VegetableController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Vegetable
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vegetables.ToListAsync());
        }

        // GET: Vegetable/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = await _context.Vegetables.SingleOrDefaultAsync(m => m.id == id);
            if (vegetable == null)
            {
                return NotFound();
            }

            return View(vegetable);
        }

        // GET: Vegetable/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vegetable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,available,description,name")] Vegetable vegetable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vegetable);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vegetable);
        }

        // GET: Vegetable/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = await _context.Vegetables.SingleOrDefaultAsync(m => m.id == id);
            if (vegetable == null)
            {
                return NotFound();
            }
            return View(vegetable);
        }

        // POST: Vegetable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("id,available,description,name")] Vegetable vegetable)
        public IActionResult Edit(int id, [Bind("id,available,description,name")] Vegetable vegetable)
        {
            if (id != vegetable.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UpdateVegetableTask newUpdateVegetableTask = new UpdateVegetableTask(vegetable);
                TenHourExecutionManager.AddNewTask(newUpdateVegetableTask);

                //CE CODE SE SITUE MAINTENANT DANS UpdateVegetableTask
                /*try
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
                }*/
                return RedirectToAction("Index");
            }
            return View(vegetable);
        }

        /*
         * ON NE SUPPRIME JAMAIS LES SANDWICHES
         * 
         * // GET: Vegetable/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = await _context.Vegetables.SingleOrDefaultAsync(m => m.id == id);
            if (vegetable == null)
            {
                return NotFound();
            }

            return View(vegetable);
        }

        // POST: Vegetable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vegetable = await _context.Vegetables.SingleOrDefaultAsync(m => m.id == id);
            _context.Vegetables.Remove(vegetable);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VegetableExists(int id)
        {
            return _context.Vegetables.Any(e => e.id == id);
        }*/
    }
}
