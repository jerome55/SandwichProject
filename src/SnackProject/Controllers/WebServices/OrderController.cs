using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnackProject.Models;
using SnackProject.Data;
using SnackProject.Models.Communication;

namespace SnackProject.Controllers.WebServices
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        public static Dictionary<string, Order> orderToValidate = new Dictionary<string, Order>();
        ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<CommWrap<OrderGuid_Com>> Post([FromBody]OrderCompany_Com communication)
        {
            Order orderAdd = communication.Order_com;
            Company company = communication.Company_com;

            Company companyDB = null;
            List<Company> companyList = _context.Companies.Where(q => q.Id == company.Id && q.Chkcode == company.Chkcode).ToList();

            if(companyList.Count == 0)
            {
                return new CommWrap<OrderGuid_Com>{ RequestStatus = 0 };
            }
            else
            {
                companyDB = companyList.First();

                //Déterminer la date de la commande (seule la date server-side fait foi) 
                DateTime now = DateTime.Now;
                DateTime delivreryDate;
                if (now.Hour >= 10)
                {
                    delivreryDate = DateTime.Today.AddDays(1.0);
                }
                else
                {
                    delivreryDate = DateTime.Today;
                }

                //Si commande existe déjà pour cette date, la récupérer
                List<Order> orders = companyDB.Orders.Where(q => q.DateOfDelivery.Equals(delivreryDate)).ToList();

                Order orderDB = null;
                if(orders.Count == 0)
                {
                    orderDB = new Order { DateOfDelivery = delivreryDate, OrderLines = new List<OrderLine>() };

                    companyDB.Orders.Add(orderDB);
                }
                else
                {
                    orderDB = orders.First();
                }
                
                //Boucler sur chaque OrderLine de l'objet Order reçu par le serveur
                for (int i=0; i<orderAdd.OrderLines.Count; i++)
                {
                    //Rafraichir les anciennes données avec des nouvelles par mesure de sécurité (les commandes 
                    //aux alentours de 10h sont susceptibles d'être altérées)
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Price = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Price;
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Name = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Name;
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Description = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Description;

                    for (int j=0; j<orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.Count; j++)
                    {
                        orderAdd.OrderLines.ElementAt(i).VegetablesPrice = _context.Menus.First().VegetablesPrice;
                        orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Name = _context.Vegetables.Where(v => v.Id == orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Name;
                        orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Description = _context.Vegetables.Where(v => v.Id == orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Description;
                    }
               
                    //Ajouter chaque OrderLine de l'object Order reçu par le serveur
                    orderDB.AddOrderLine()
                }

                orderDB.AddOrderLine(orderLineAddList);

                _context.SaveChanges();

                return new CommWrap<Order> { RequestStatus = 1, Content = orderDB };
            }
        }

        [HttpPost]
        [Route("api/Order/Confirm")]
        public async Task<void> Confirm([FromBody]OrderCompany_Com communication)
        {
        }
}