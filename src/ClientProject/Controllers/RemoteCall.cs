using ClientProject.Models;
using ClientProject.Models.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientProject.Controllers
{
    public sealed class RemoteCall
    {
        private static volatile RemoteCall Instance = null;
        private static readonly object syncLock = new object();

        private HttpClient client;

        private RemoteCall()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55367/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static RemoteCall GetInstance()
        {
            //Mettre un lock ici
            if(Instance == null)
            {
                lock (syncLock)
                {
                    if(Instance == null) { Instance = new RemoteCall(); }
                }
            }
            return Instance;
        }
        
        public async Task<CommWrap<Company>> RegisterCompany(Company company)
        {
            CommWrap<Company> responseReturn = null;

            HttpResponseMessage response = await this.client.PostAsJsonAsync("api/RegisterCompany", company);
            if (response.IsSuccessStatusCode)
            {
                responseReturn = await response.Content.ReadAsAsync<CommWrap<Company>>();
            }
            
            return responseReturn;
        }

        /*public static CheckCompanyRegister()
        {

        }*/


    }
}
