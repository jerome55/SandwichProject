using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClientProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ClientProject.InfoProviders;
using ClientProject.Services;
using Microsoft.Extensions.Logging;
using ClientProject.Models.CompanyRegisterViewModels;
using ClientProject.Models.Communication;
using ClientProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ClientProject.Controllers.Remote;

namespace ClientProject.Controllers
{
    [Authorize]
    public class CompanyRegisterController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ActivationInformant _activationInformant;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public CompanyRegisterController(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            RoleManager<IdentityRole> roleManager,
            ActivationInformant activationInformant,
            ApplicationDbContext context,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _activationInformant = activationInformant;
            _context = context;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        //
        // GET: /CompanyRegister
        [HttpGet]
        [AllowAnonymous]
        [Route("CompanyRegister/Index")]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (await _activationInformant.IsRegistered())
            {
                return RedirectToLocal(returnUrl);
            }
            return View();
        }

        //
        // POST: /CompanyRegister/Register 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("CompanyRegister/Index")]
        public async Task<IActionResult> Index(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (await _activationInformant.IsRegistered())
            {
                return RedirectToLocal(returnUrl);
            }

            if (ModelState.IsValid)
            {
                var company = new Company { Name = model.CompanyName, NbEmployees = model.NumberOfEmployee, Mail = model.Email, Address = model.AddressNumber + " " + model.AddressStreet + ", " + model.AddressBox + ", " + model.AddressPostalCode + ", " + model.AddressCity + ", " + model.AddressCountry, Status = false };
                var manager = new Employee { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Wallet = (decimal)0.00, Company = company };

                RemoteCall remoteCaller = RemoteCall.GetInstance();
                CommWrap<Company> responseCompany = await remoteCaller.RegisterCompany(company);

                if(responseCompany.RequestStatus == 1)
                {
                    company.Id = responseCompany.Content.Id;
                    company.ChkCode = responseCompany.Content.ChkCode;
                    var result = await _userManager.CreateAsync(manager, model.Password);
                    if (result.Succeeded)
                    {
                        IdentityRole role = new IdentityRole { Name = "Responsable", NormalizedName = "RESPONSABLE" };
                        bool roleExist = await _roleManager.RoleExistsAsync(role.NormalizedName);
                        if (!roleExist)
                        {
                            IdentityResult roleResult = await _roleManager.CreateAsync(role);
                            if (roleResult.Succeeded)
                                await _userManager.AddToRoleAsync(manager, role.Name);
                        }
                        else {
                            await _userManager.AddToRoleAsync(manager, role.Name);
                        }

                        return RedirectToLocal(returnUrl);
                    }
                    AddErrors(result);
                }
                AddErrors("Registration failed on the Snack side");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private void AddErrors(String errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
        }
        
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}