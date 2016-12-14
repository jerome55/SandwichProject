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
    [Route("api/Sandwich")]
    public class SandwichController : Controller
    {
        ApplicationDbContext _context;

        public SandwichController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<CommWrap<Sandwich>> Get(int id)
        {

            Sandwich san = _context.Sandwiches.Where(q => q.Id == id).First();
            int sucess;
            if (san == null)
            {
                sucess = 0;
            }
            else
            {
                sucess = 1;
            }
            return new CommWrap<Sandwich> { Content = san ,RequestStatus = sucess };
        }
    }
}