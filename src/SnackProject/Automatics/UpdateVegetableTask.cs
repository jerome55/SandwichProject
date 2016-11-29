using Microsoft.EntityFrameworkCore;
using SnackProject.Data;
using SnackProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Automatics
{
    public class UpdateVegetableTask : ExecutionTask
    {
        private ApplicationDbContext _context;
        private Vegetable vegetable;

        public UpdateVegetableTask(Vegetable vegetable)
        {
            this.vegetable = vegetable;
        }

        public async void Execute(ApplicationDbContext context)
        {
            _context = context;
            try
            {
                _context.Update(vegetable);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VegetableExists(vegetable.id))
                {
                    throw new Exception("Vegetable Not Found");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool VegetableExists(int id)
        {
            return _context.Vegetables.Any(e => e.id == id);
        }

        public string GetKey()
        {
            return "UVG" + vegetable.id;
        }
    }
}
