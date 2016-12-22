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
        public static Dictionary<string, OrderCompany_Com> orderToValidate = new Dictionary<string, OrderCompany_Com>();
        ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


        /**
         * Au moment de confirmer son panier, le client envoye son objet Order qui correspond au panier ainsi que 
         * l'objet Company correspondant � l'entreprise � laquelle il appartient.
         * 
         * Les informations de dates et de prix du cot� client ne sont peut �tre pas correctes ou � jour. Ainsi,
         * ce sont les informations du cot� serveur qui font foi.
         * 
         * Un objet Order corrig� avec les informations du serveur est renvoy� au client pour qu'ils aient tous deux
         * la m�me version des donn�es. Ce m�me objet est aussi stock� sur le serveur en attente d'une r�ponse du client.
         * 
         * Apr�s avoir effectu� ses propres v�rifications de son cot� (cr�dits suffisants), le client confirme 
         * d�finitivement ou abandonne la commande.
         *                                                                  ------
         *  Client  ---------------Company, Order --------------)  Serveur       |
         *  Client  (--------------Updated_Order-----------------  Serveur       |--> [Post] api/Order
         *                                                                       |
         *       -> Order stock�e temporaire en attente d'une validation client  |
         *                                                                  ------                                                            
         *                                                                  
         * [IF-                                                                 
         *  -OK](Cr�dits suffisants)                                        ------
         *  Client  ------------{{GUID_Of_Order}OK}-------------)  Serveur       |
         *                                                                       |
         *                -> L'Order stock�e est d�finitvement sauvegard� en DB  | 
         *                                                                       |--> [Post] api/Order/Confirm
         *  -KO](Cr�dits insuffisants)                                           |
         *  Client  ------------{{_____________}KO}-------------)  Serveur       |
         *                -> L'Order stock�e est abandonn�e (ou timeout)         |
         *                                                                  ------
         **/
        [HttpPost]
        public async Task<CommWrap<OrderCompany_Com>> Post([FromBody]OrderCompany_Com communication)
        {
            Order orderClient = communication.Order_com;
            Company company = communication.Company_com;

            Company companyDB = null;
            List<Company> companyList = _context.Companies.Where(q => q.Id == company.Id && q.Chkcode == company.Chkcode).ToList();

            if (companyList.Count == 0)
            {
                return new CommWrap<OrderCompany_Com> { RequestStatus = 0 };
            }
            else
            {
                companyDB = companyList.First();

                //D�terminer la date de la commande (seule la date server-side fait foi) 
                DateTime now = DateTime.Now;
                DateTime deliveryDate;
                if (now.Hour >= 10)
                {
                    deliveryDate = DateTime.Today.AddDays(1.0);
                }
                else
                {
                    deliveryDate = DateTime.Today;
                }

                orderClient.DateOfDelivery = deliveryDate;

                //Rafraichir les anciennes donn�es avec des nouvelles par mesure de s�curit� (les commandes 
                //aux alentours de 10h sont susceptibles d'�tre alt�r�es)
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < orderClient.OrderLines.Count; i++)
                        {
                            orderClient.OrderLines.ElementAt(i).Sandwich.Price = _context.Sandwiches.Where(s => s.Id == orderClient.OrderLines.ElementAt(i).Sandwich.Id).First().Price;
                            orderClient.OrderLines.ElementAt(i).Sandwich.Name = _context.Sandwiches.Where(s => s.Id == orderClient.OrderLines.ElementAt(i).Sandwich.Id).First().Name;
                            orderClient.OrderLines.ElementAt(i).Sandwich.Description = _context.Sandwiches.Where(s => s.Id == orderClient.OrderLines.ElementAt(i).Sandwich.Id).First().Description;

                            for (int j = 0; j < orderClient.OrderLines.ElementAt(i).OrderLineVegetables.Count; j++)
                            {
                                orderClient.OrderLines.ElementAt(i).VegetablesPrice = _context.Menus.First().VegetablesPrice;
                                orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Name = _context.Vegetables.Where(v => v.Id == orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Name;
                                orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Description = _context.Vegetables.Where(v => v.Id == orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Description;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return new CommWrap<OrderCompany_Com> { RequestStatus = 0 };
                    }
                
                return new CommWrap<OrderCompany_Com> { RequestStatus = 1, Content = communication};
            }
        }

        /*[HttpPost]
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

                //D�terminer la date de la commande (seule la date server-side fait foi) 
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

                //Si commande existe d�j� pour cette date, la r�cup�rer
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
                
                //Boucler sur chaque OrderLine de l'objet Order re�u par le serveur
                for (int i=0; i<orderAdd.OrderLines.Count; i++)
                {
                    //Rafraichir les anciennes donn�es avec des nouvelles par mesure de s�curit� (les commandes 
                    //aux alentours de 10h sont susceptibles d'�tre alt�r�es)
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Price = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Price;
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Name = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Name;
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Description = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Description;

                    for (int j=0; j<orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.Count; j++)
                    {
                        orderAdd.OrderLines.ElementAt(i).VegetablesPrice = _context.Menus.First().VegetablesPrice;
                        orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Name = _context.Vegetables.Where(v => v.Id == orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Name;
                        orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Description = _context.Vegetables.Where(v => v.Id == orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Description;
                    }

                    //Ajouter chaque OrderLine de l'object Order re�u par le serveur
                    orderDB.AddOrderLine();
                }

                orderDB.AddOrderLine(orderLineAddList);

                _context.SaveChanges();

                return new CommWrap<Order> { RequestStatus = 1, Content = orderDB };
            }
        }*/

        [HttpPost]
        [Route("api/Order/Confirm")]
        public async Task<void> Confirm([FromBody]OrderCompany_Com communication)
        {
        }
}