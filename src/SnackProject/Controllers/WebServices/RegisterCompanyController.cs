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
    [Route("api/RegisterCompany")]
    public class RegisterCompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterCompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        /*[HttpGet("{id}", Name = "GetCompany")]
        public IActionResult GetById(int id)
        {
            var item = _context.Companies.SingleOrDefault(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }*/

        [HttpPost]
        public async Task<CommWrap<Company>> Post(Company newCompany)
        {
            try
            {
                _context.Add(newCompany);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            CommWrap<Company> response = new CommWrap<Company> { RequestStatus = 1, Content = newCompany };
            return response;

            //return newCompany;

            //return CreatedAtRoute("GetCompany", new { id = newCompany.Id }, newCompany);

        }
    }
}