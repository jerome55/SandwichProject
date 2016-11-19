using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnackProject.Data;
using SnackProject.Models;
using Microsoft.EntityFrameworkCore;

namespace SnackProject.Automatics
{
    public class UpdateSandwichTask : ExecutionTask
    {
        private SnackContext _context;
        private Sandwich sandwich;

        public UpdateSandwichTask(Sandwich sandwich)
        {
            this.sandwich = sandwich;
        }

        public async void Execute(SnackContext context)
        {
            _context = context;
            try
            {
                _context.Update(sandwich);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SandwichExists(sandwich.id))
                {
                    throw new Exception("Sandwich Not Found");     
                }
                else
                {
                    throw;
                }
            }
        }

        private bool SandwichExists(int id)
        {
            return _context.Sandwiches.Any(e => e.id == id);
        }

        public string GetKey()
        {
            return "usd"+sandwich.id;
        }
    }
}
