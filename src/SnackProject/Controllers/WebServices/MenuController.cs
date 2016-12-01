using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnackProject.Data;
using SnackProject.Models;

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
        public async Task<Menu> Get()
        {

            ICollection<Sandwich> sand = (ICollection<Sandwich>)_context.Sandwiches
                                            .Where(q => q.available == true)
                                            .ToList();
            ICollection<Vegetable> veg = _context.Vegetables
                                            .Where(q => q.available == true)
                                            .ToList();

            Menu menu = new Menu { id = 1, vegetablesPrice  =5,sandwiches= sand, vegetables = veg };
            return menu ;
        }


    }
}