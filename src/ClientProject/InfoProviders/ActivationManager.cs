using ClientProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.InfoProviders
{
    public class ActivationManager
    {
        private static volatile ActivationManager instance;
        private static object syncRoot = new object();

        private ApplicationDbContext _context;

        private ActivationManager(ApplicationDbContext context)
        {
            this._context = context;
        }

        public static ActivationManager GetInstance(ApplicationDbContext context)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null) { instance = new ActivationManager(context); }
                }
            }
            return instance;
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
