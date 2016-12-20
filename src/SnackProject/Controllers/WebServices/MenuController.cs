using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnackProject.Data;
using SnackProject.Models;
using SnackProject.Models.Communication;

namespace SnackProject.Controllers.WebServices
{
    [Produces("application/json")]
    [Route("api/Menu")]
    public class MenuController : Controller
    {
        ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<CommWrap<Menu>> Get()
        {
            ICollection<Sandwich> sand = _context.Sandwiches
                                            .Where(q => q.Available == true)
                                            .ToList();
            ICollection<Vegetable> veg = _context.Vegetables
                                            .Where(q => q.Available == true)
                                            .ToList();

            List<Menu> menus = _context.Menus.ToList();

            CommWrap<Menu> commRes = null;
            if(menus.Count == 0)
            {
                commRes = new CommWrap<Menu> { RequestStatus = 0};
            }
            else
            {
                Menu menu = new Menu { Id = menus[0].Id, VegetablesPrice = menus[0].VegetablesPrice, Sandwiches = sand, Vegetables = veg };
                commRes = new CommWrap<Menu> { RequestStatus = 1, Content = menu };
            }

            return commRes;
        }
    }
}