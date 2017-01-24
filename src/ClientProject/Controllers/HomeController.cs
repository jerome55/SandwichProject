using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClientProject.InfoProviders;

namespace ClientProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ActivationInformant _activationInformant;

        public HomeController(ActivationInformant activationInformant)
        {
            _activationInformant = activationInformant;
        }


        public async Task<IActionResult> Index()
        {
            if (await _activationInformant.IsRegistered() == false)
            {
                return RedirectToAction(nameof(CompanyRegisterController.Index), "CompanyRegister");
            }
            else
            {
                return RedirectToAction("Index","Menu");
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
