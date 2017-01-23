using ClientProject.Models;
using ClientProject.Models.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientProject.Controllers.Remote
{
    public sealed class RemoteCall
    {
        private static volatile RemoteCall Instance = null;
        private static readonly object syncLock = new object();

        private HttpClient client;

        private RemoteCall()
        {
            client = new HttpClient();
            //Timeout change for debugging purpose
            client.Timeout = new TimeSpan(0, 40, 0);
            
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

        public async Task<CommWrap<Menu>> GetMenu()
        {
            CommWrap<Menu> responseReturn = null;

            HttpResponseMessage response = await this.client.GetAsync("api/Menu");
            if (response.IsSuccessStatusCode)
            {
                responseReturn = await response.Content.ReadAsAsync<CommWrap<Menu>>();
            }

            return responseReturn;
        }

        //Permet de recuperer un sandwich par son id
        public async Task<CommWrap<Sandwich>> GetSandwichById(int id)
        {
            CommWrap<Sandwich> responseReturn = null;

            HttpResponseMessage response = await this.client.GetAsync("api/Sandwich/" + id);
            if (response.IsSuccessStatusCode)
            {
                responseReturn = await response.Content.ReadAsAsync<CommWrap<Sandwich>>();
            }

            return responseReturn;
        }

        //Permet de recuperer une cruditée par son id
        public async Task<CommWrap<Vegetable>> GetVegetableById(int id)
        {
            CommWrap<Vegetable> responseReturn = null;

            HttpResponseMessage response = await this.client.GetAsync("api/Vegetable/" + id);
            if (response.IsSuccessStatusCode)
            {
                responseReturn = await response.Content.ReadAsAsync<CommWrap<Vegetable>>();
            }

            return responseReturn;
        }

        
        public async Task<CommWrap<OrderGuid_Com>> SendOrder(Order orderToSend, Company company)
        {
            CommWrap<OrderGuid_Com> responseReturn = null;
            OrderCompany_Com orderCompany = new OrderCompany_Com { Order_com = orderToSend, Company_com = company };

            HttpResponseMessage response = await this.client.PostAsJsonAsync("api/Order", orderCompany);
            if (response.IsSuccessStatusCode)
            {
                responseReturn = await response.Content.ReadAsAsync<CommWrap<OrderGuid_Com>>();
            }

            return responseReturn;
        }

        public async Task<CommWrap<string>> ConfirmOrder(bool validate, string orderGuid)
        {
            CommWrap<string> responseReturn = null;
            CommWrap<string> validationMessage = new CommWrap<string> { Content = orderGuid };
            if(validate == true) {
                validationMessage.RequestStatus = 1;
            }
            else {
                validationMessage.RequestStatus = 0;
            }

            HttpResponseMessage response = await this.client.PostAsJsonAsync("api/Order/Confirm", validationMessage);
            if (response.IsSuccessStatusCode)
            {
                responseReturn = await response.Content.ReadAsAsync<CommWrap<string>>();
            }

            return responseReturn;
        }
        public async Task<int> SendOrderForDelete(OrderCompany_Com orderCompCom){
            int i = -1;
            HttpResponseMessage response = await this.client.PutAsJsonAsync("api/Order",orderCompCom);
            if (response.IsSuccessStatusCode)
            {
                i = await response.Content.ReadAsAsync<int>();
            }
            return i;
        }
    }
}
