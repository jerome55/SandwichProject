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
using System.Diagnostics;
using SnackProject.Models.VegetableViewModels;

namespace SnackProject.Controllers
{
    public class VegetableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VegetableController(ApplicationDbContext context)
        {
            _context = context;    
        }


        //Tri l'affichage des crudit�s par ordre de disponibilit�
        // GET: Crudit�
        public IActionResult Index()
        {
            var i = TenHourExecutionManager.context.Vegetables.OrderBy(x => x.Available ? 0 : 1);
            //Si le contexte contient des entr�es ajout�es ou modifi�es du type vegetable, r�cup�re les crudit�s depuis le change tracker
            if (TenHourExecutionManager.context.ChangeTracker.Entries<Vegetable>().Any())
            {
                var j = TenHourExecutionManager.context.ChangeTracker.Entries<Vegetable>().Select(x => x.Entity as Vegetable).AsQueryable();
                i = j.OrderBy(x => x.Available ? 0 : 1);
            }

            IList<Vegetable> vegetables = i.ToList();

            //r�cup�re le prix des crudit�s depuis le contexte statique ou depuis le changeTracker du contexte statique si celui-ci a �t� modifi�.
            decimal VegetablesPrice = TenHourExecutionManager.context.Menus.First().VegetablesPrice;
            if (TenHourExecutionManager.context.ChangeTracker.Entries<Menu>().Any())
            {
                VegetablesPrice = TenHourExecutionManager.context.ChangeTracker.Entries<Menu>().First().Entity.VegetablesPrice;
            }
            VegetableViewModel model = new VegetableViewModel { Vegetables = vegetables, VegetablesPrice = VegetablesPrice };

            return View(model);
        }

        //mise � jour du prix dans le contexte statique
        [HttpPost]
        public IActionResult Index(VegetableViewModel model)
        {
            if(model.VegetablesPrice >= 0)
            {
                Menu menu = TenHourExecutionManager.context.Menus.First();
                if (TenHourExecutionManager.context.ChangeTracker.Entries<Menu>().Any())
                {
                    menu = TenHourExecutionManager.context.ChangeTracker.Entries<Menu>().First().Entity;
                }
                if (menu != null)
                {
                    menu.VegetablesPrice = model.VegetablesPrice;
                }
            }
            return Index();
        }


        // GET: Vegetable/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = TenHourExecutionManager.context.Vegetables.SingleOrDefault(m => m.Id == id);
            if (TenHourExecutionManager.context.ChangeTracker.Entries<Vegetable>().Any())
            {
                vegetable = TenHourExecutionManager.context.ChangeTracker.Entries<Vegetable>().Select(x => x.Entity as Vegetable).AsQueryable().SingleOrDefault(m => m.Id == id);
            }

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
        /* set la crudit� � available
         * Ajoute une crudit� avec l'�tat added dans le contexte static
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Available,Description,Name")] Vegetable vegetable)
        {
            if (ModelState.IsValid)
            {
                vegetable.Available = true;
                TenHourExecutionManager.context.Entry(vegetable).State = EntityState.Added;
                return RedirectToAction("Index");
            }
            return View(vegetable);
        }

        // GET: Vegetable/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vegetable = TenHourExecutionManager.context.ChangeTracker.Entries<Vegetable>().Select(x => x.Entity as Vegetable).AsQueryable().SingleOrDefault(m => m.Id == id);
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
        public IActionResult Edit(int id, [Bind("Id,Available,Description,Name")] Vegetable vegetable)
        {
            if (id != vegetable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var oldEntity = TenHourExecutionManager.context.ChangeTracker.Entries<Vegetable>().Select(x => x.Entity as Vegetable).AsQueryable().SingleOrDefault(m => m.Id == id);
                //dans le cas d'une entr�e ajout�e et modifi�e sans avoir encore �t� valid�e sur la bd, les modifications se font en dur (probl�me de r�f�rence des ids)
                if (TenHourExecutionManager.context.ChangeTracker.Entries().LastOrDefault().Context.Entry(oldEntity).State == EntityState.Added)
                {
                    oldEntity.Available = vegetable.Available;
                    oldEntity.Description = vegetable.Description;
                    oldEntity.Name = vegetable.Name;
                }
                //dans le cas de la modification d'une entit� d�j� pr�sente sur la bd, l'entit� du contexte static est d�tach�e et la nouvelle est ajout�e comme updated.
                else
                {
                    TenHourExecutionManager.context.Entry(oldEntity).State = EntityState.Detached;
                    TenHourExecutionManager.context.Entry(vegetable).State = EntityState.Modified;
                }

                return RedirectToAction("Index");
            }
            return View(vegetable);
        }

        //On ne supprime jamais une crudit�
    }
}
