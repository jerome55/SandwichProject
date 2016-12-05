using ClientProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.InfoProviders
{
    public class ActivationInformant
    {
        private ApplicationDbContext _context;

        public ActivationInformant(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> IsRegistered()
        {
            List<Models.Company> companiesList = await this._context.Companies.ToListAsync();
            if (companiesList.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
