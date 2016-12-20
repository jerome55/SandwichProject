using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnackProject.Data;
using SnackProject.Models.Communication;
using SnackProject.Models;

namespace SnackProject.Controllers.WebServices
{
    [Produces("application/json")]
    [Route("api/Vegetable")]
    public class VegetableController : Controller
    {
        ApplicationDbContext _context;

        public VegetableController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/Vegetable/}{id}")]
        //Permet de recuperer une cruditée par son id
        public async Task<CommWrap<Vegetable>> Get(int id)
        {

            Vegetable veg = _context.Vegetables.Where(q => q.Id == id).First();
            int sucess;
            if (veg == null)
            {
                sucess = 0;
            }
            else
            {
                sucess = 1;
            }
            return new CommWrap<Vegetable> { Content = veg, RequestStatus = sucess };
        }
    }
}