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

namespace ClientProject.Controllers
{
    [Authorize]
    public class CompanyRegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ActivationInformant _activationInformant;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public CompanyRegisterController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ActivationInformant activationInformant,
            ApplicationDbContext context,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _activationInformant = activationInformant;
            _context = context;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // GET: /Account/RegisterManager
        [HttpGet]
        [AllowAnonymous]
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
        // POST: /Account/RegisterManager
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (await _activationInformant.IsRegistered())
            {
                return RedirectToLocal(returnUrl);
            }

            if (ModelState.IsValid)
            {
                var company = new Company { Name = model.CompanyName, NbEmployees = model.NumberOfEmployee, Mail = model.Email, Address = model.AddressNumber + " " + model.AddressStreet + ", " + model.AddressBox + ", " + model.AddressPostalCode + ", " + model.AddressCity + ", " + model.AddressCountry, Status = false };
                var manager = new Employee { FirstName = model.FirstName, LastName = model.LastName, Wallet = (decimal)0.00, Company = company };
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Employee = manager };
                manager.ApplicationUser = user;

                RemoteCall remoteCaller = RemoteCall.GetInstance();
                CommWrap<Company> responseCompany = await remoteCaller.RegisterCompany(company);

                if(responseCompany.RequestStatus == 1)
                {
                    company.Id = responseCompany.Content.Id;
                    company.ChkCode = responseCompany.Content.ChkCode;
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    AddErrors(result);
                }
                AddErrors("Registration failed on the Snack side");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        
        /*//
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterManager(RegisterCompanyViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }*/

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