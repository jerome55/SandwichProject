using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnackProject.Data;
using Microsoft.EntityFrameworkCore;

namespace SnackProject.Controllers.WebServices
{
    [Produces("application/json")]
    [Route("api/CheckCompanyRegister")]
    public class CheckCompanyRegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckCompanyRegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async Task<bool> Post([FromBody]int id, [FromBody]string chkcode)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                if (company.Chkcode == chkcode && company.Status == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}