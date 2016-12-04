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

        /*public async Task<bool> IsActivated()
        {

            List<Models.Company> companiesList = await this._context.Companies.ToListAsync();
            if(companiesList.Count != 0) {
                if(companiesList[0].Status == true) { return true; }
                else { return false; } 
            }
            else {
                return false;
            }
        }*/


        /* public async Task<bool> CheckStatusRegistration()
         {
             List<Models.Company> companiesList = await this._context.Companies.ToListAsync();
             if (companiesList.Count != 0)
             {
                 if (companiesList[0].status)
                 {
                     return true;
                 }
                 else
                 {
                     //distantStatusCall WebService CheckStatus
                     //If(distantStatus==true)
                     ////Update localdb change status
                     ////Return true
                     //Else
                     ////Return False
                 }
             }
         }*/
    }
}
