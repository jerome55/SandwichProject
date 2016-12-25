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
using System.Diagnostics;

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
        
        // GET: Menu
        [AllowAnonymous]
        [Route("Menu/Index")]
        public async Task<IActionResult> Index()
        {
            RemoteCall remoteCaller = RemoteCall.GetInstance();
            CommWrap<Menu> responseMenu = await remoteCaller.GetMenu();

            MenuViewModel menuViewModel = new MenuViewModel();

            if (responseMenu.RequestStatus == 1)
            {
                Menu menu = responseMenu.Content;

                menuViewModel.ListSandwiches = menu.Sandwiches.ToList();
                menuViewModel.SelectedSandwich = null;

                menuViewModel.VegetablesPrice = menu.VegetablesPrice;
                List<VegWithChkBxViewModel> listVegWithChkBxViewModels = new List<VegWithChkBxViewModel>();
                foreach (Vegetable veg in menu.Vegetables) {
                    VegWithChkBxViewModel vegWithChkBxViewModel = new VegWithChkBxViewModel { Checked = false, Id = veg.Id, Name = veg.Name, Description = veg.Description };
                    listVegWithChkBxViewModels.Add(vegWithChkBxViewModel);
                }
                menuViewModel.ListVegetablesWithCheckBoxes = listVegWithChkBxViewModels;

                return View(menuViewModel);
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
        }

        
        // POST: OrderLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employe, Responsable")]
        [Route("Menu/Index")]
        public async Task<IActionResult> Index(MenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                /*String selectedVegetables = "";
                foreach (VegWithChkBxViewModel vegWithChkBx in model.ListVegetablesWithCheckBoxes)
                {
                    if(vegWithChkBx.Checked == true)
                    {
                        selectedVegetables += vegWithChkBx.Id + "/";
                    }
                }
                Debug.WriteLine("-------------- selectedSandwich : " + model.SelectedSandwich);
                Debug.WriteLine("-------------- selectedVegetables : " + selectedVegetables);
                */
                if(model.SelectedSandwich != null)
                {
                    OrderLine newOrderLine = new OrderLine { Quantity = 1, VegetablesPrice = model.VegetablesPrice, OrderLineVegetables = new List<OrderLineVegetable>() };

                    Sandwich selectedSandwich = model.ListSandwiches.Where(s => s.Id == Int32.Parse(model.SelectedSandwich)).First();
                    newOrderLine.Sandwich = selectedSandwich;

                    foreach (VegWithChkBxViewModel vegWithChkBx in model.ListVegetablesWithCheckBoxes)
                    {
                        if (vegWithChkBx.Checked == true)
                        {
                            VegWithChkBxViewModel selectedVegWithChkBx = model.ListVegetablesWithCheckBoxes.Where(v => v.Id == vegWithChkBx.Id).First();
                            Vegetable selectedVegetable = new Vegetable { Id = selectedVegWithChkBx.Id, Name = selectedVegWithChkBx.Name, Description = selectedVegWithChkBx.Description };
                            newOrderLine.AddVegetable(selectedVegetable);
                        }
                    }

                    AddOrderLineToCartSession(newOrderLine);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Employe, Responsable")]
        public async Task<IActionResult> InvalidateCartSession()
        {
            HttpContext.Session.SetString("cart", "");

            return Redirect(Request.Headers["Referer"].ToString());
        }

        /*
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
        */

        [Authorize(Roles = "Employe, Responsable")]
        [Route("Menu/ValidateSessionCart")]
        public async Task<IActionResult> ValidateSessionCart()
        {
            Order cartOrder = ShoppingCart.GetCartContent(User, HttpContext);

            Company companyDb = _context.Companies.First();
            //Si la company n'est visible en DB, c'est qui il y a un gros problème.
            if (companyDb == null)
            {
                return NotFound();//Ou page d'erreur
            }

            CommWrap<OrderGuid_Com> comm =  await RemoteCall.GetInstance().SendOrder(cartOrder, companyDb);
            if(comm.RequestStatus == 0)
            {
                //Si une erreur est survenue sur le serveur : company pas trouvée, erreur Db, ...
                return NotFound();//Or pas d'erreur
            }
            Order orderUpdatedByServ = comm.Content.Order_com;
            string orderUpdatedGuid = comm.Content.Guid_com;

            string employeeId = _userManager.GetUserId(User);
            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == employeeId);

            bool reussite = employee.DebitWallet(orderUpdatedByServ.TotalAmount);
            if (reussite == false)
            {
                //L'employé n'a pas suffisement d'argent.
                CommWrap<string> commCancellation = await RemoteCall.GetInstance().ConfirmOrder(false, orderUpdatedGuid);
                if(commCancellation.RequestStatus != 0)
                {
                    //Il faut afficher à l'utilisateur qu'il n'a pas assez d'argent.
                    //return
                }
                //L'annulation n'a pas aboutie, si on arrive ici, c'est que quelque 
                //chose d'inattendu s'est produit ou peut être que la requête a été 
                //appelée manuelle par l'utilisateur.
                //return
            }

            //L'employé avait suffisement de crédit et sa commande se confirme.
            CommWrap<string> commConfirmation = await RemoteCall.GetInstance().ConfirmOrder(true, orderUpdatedGuid);
            if(commConfirmation.RequestStatus != 0)
            {
                //Tout s'est bien passé, on peut vider le panier
                ShoppingCart.UpdateCartContent(HttpContext, null);
                return RedirectToAction("Index");
            }

            //La confirmation n'a pas aboutie, si on arrive ici, c'est que quelque 
            //chose d'inattendu s'est produit ou peut être que la requête a été 
            //appelée manuelle par l'utilisateur.
            //return
            return RedirectToAction("Index");//temporaire juste le temps des tests
        }



        private bool OrderLineExists(int id)
        {
            return _context.OrderLines.Any(e => e.Id == id);
        }
        
        private void AddOrderLineToCartSession(OrderLine add)
        {
            Order cartOrder = ShoppingCart.GetCartContent(User, HttpContext);

            cartOrder.AddOrderLine(add);

            ShoppingCart.UpdateCartContent(HttpContext, cartOrder);
        }
        
        // pre : employer contient ses orders.
        private Order GetCurrentOrder(Employee employee)
        {
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

        private void AddErrors(String errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
        }
    }
}
