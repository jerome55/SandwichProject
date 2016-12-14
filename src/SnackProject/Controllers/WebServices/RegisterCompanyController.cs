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
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public RegisterCompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<CommWrap<Company>> Post([FromBody]Company newCompany)
        {
            CommWrap<Company> response = new CommWrap<Company> { RequestStatus = 0, Content = newCompany };
            try {
                newCompany.Status = true;
                newCompany.Chkcode = RandomString(10);
                _context.Add(newCompany);
                await _context.SaveChangesAsync();
                response = new CommWrap<Company> { RequestStatus = 1, Content = newCompany };
            }
            catch (Exception) {
            }
            
            return response;
        }
    }
}