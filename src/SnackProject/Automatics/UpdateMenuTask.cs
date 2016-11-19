using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnackProject.Data;
using SnackProject.Models;
using Microsoft.EntityFrameworkCore;

namespace SnackProject.Automatics
{
    public class UpdateMenuTask : ExecutionTask
    {
        private SnackContext _context;
        private Menu menu;

        public UpdateMenuTask(Menu menu)
        {
            this.menu = menu;
        }

        public async void Execute(SnackContext context)
        {
            _context = context;
            try
            {
                _context.Update(menu);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(menu.id))
                {
                    throw new Exception("Menu Not Found");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.id == id);
        }

        public string GetKey()
        {
            return "UM" + menu.id;
        }
    }
}
