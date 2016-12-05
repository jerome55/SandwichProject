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
using ClientProject.Services;
using Microsoft.Extensions.Logging;
using ClientProject.Models.EmployeesManagementViewModels;
using System.Diagnostics;

namespace ClientProject.Controllers
{
    public class EmployeesManagementController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        
        public EmployeesManagementController(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        //
        // GET: /EmployeesManagement
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        //
        // GET: /EmployeesManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        //
        // POST: /EmployeesManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Wallet = model.Wallet };
                var user = await _userManager.CreateAsync(employee, model.Password);
                
                if (user.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    return RedirectToAction("Index");
                }
                AddErrors(user);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: EmployeesManagement/Edit/5
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

        //
        // POST: EmployeesManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,UserName,Email")] Employee model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Employee employeeInDb = await _userManager.FindByIdAsync(id);
                    if (employeeInDb == null)
                    {
                        return NotFound();
                    }
                    employeeInDb.FirstName = model.FirstName;
                    employeeInDb.LastName = model.LastName;
                    employeeInDb.UserName = model.UserName;
                    employeeInDb.Email = model.Email;

                    await _userManager.UpdateAsync(employeeInDb);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(model.Id))
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
            return View(model);
        }

        //
        // GET: EmployeesManagement/AddToWallet/5
        public async Task<IActionResult> AddToWallet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            AddToWalletViewModel model = new AddToWalletViewModel { Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName, UserName = employee.UserName, Email = employee.Email, Wallet = employee.Wallet, AddToWallet = (decimal)0.00 };

            return View(model);
        }

        //
        // POST: EmployeesManagement/AddToWallet/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWallet(AddToWalletViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == model.Id);
                if (employee == null)
                {
                    return NotFound();
                }

                employee.Wallet += model.AddToWallet;

                _context.Update(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: EmployeesManagement/Delete/5
        /*public async Task<IActionResult> Delete(string id)
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
        }*/

        //
        // POST: EmployeesManagement/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }*/


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}