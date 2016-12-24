using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SnackProject.Models;
using SnackProject.Data;
using SnackProject.Models.Communication;
using Microsoft.EntityFrameworkCore;

namespace SnackProject.Controllers.WebServices
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        public static Dictionary<string, OrderCompany_Com> ordersToValidate = new Dictionary<string, OrderCompany_Com>();
        ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


        /**
         * Au moment de confirmer son panier, le client envoye son objet Order qui correspond au panier ainsi que 
         * l'objet Company correspondant à l'entreprise à laquelle il appartient.
         * 
         * Les informations de dates et de prix du coté client ne sont peut être pas correctes ou à jour. Ainsi,
         * ce sont les informations du coté serveur qui font foi.
         * 
         * Un objet Order corrigé avec les informations du serveur est renvoyé au client pour qu'ils aient tous deux
         * la même version des données. Ce même objet est aussi stocké sur le serveur en attente d'une réponse du client.
         * 
         * Après avoir effectué ses propres vérifications de son coté (crédits suffisants), le client confirme 
         * définitivement ou abandonne la commande.
         *                                                                  ------
         *  Client  ---------------Company, Order --------------)  Serveur       |
         *  Client  (--------------Updated_Order-----------------  Serveur       |--> [Post] api/Order
         *                                                                       |
         *       -> Order stockée temporaire en attente d'une validation client  |
         *                                                                  ------                                                            
         *                                                                  
         * [IF-                                                                 
         *  -OK](Crédits suffisants)                                        ------
         *  Client  ------------{{GUID_Of_Order}OK}-------------)  Serveur       |
         *                                                                       |
         *                -> L'Order stockée est définitvement sauvegardé en DB  | 
         *                                                                       |--> [Post] api/Order/Confirm
         *  -KO](Crédits insuffisants)                                           |
         *  Client  ------------{{_____________}KO}-------------)  Serveur       |
         *                -> L'Order stockée est abandonnée (ou timeout)         |
         *                                                                  ------
         **/
        [HttpPost]
        public async Task<CommWrap<OrderGuid_Com>> Post([FromBody]OrderCompany_Com communication)
        {
            Order orderClient = communication.Order_com;
            Company company = communication.Company_com;

            Company companyDB = null;
            List<Company> companyList = _context.Companies.Where(q => q.Id == company.Id && q.Chkcode == company.Chkcode).ToList();

            //Si la company dans l'employé prétend faire partie n'est pas trouvée, s'arrêter.
            if (companyList.Count == 0)
            {
                return new CommWrap<OrderGuid_Com> { RequestStatus = 0 };
            }
            else
            {
                companyDB = companyList.First();

                //Déterminer la date de la commande (seule la date server-side fait foi) 
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

                //Rafraichir les anciennes données avec des nouvelles par mesure de sécurité (les commandes 
                //aux alentours de 10h sont susceptibles d'être altérées)
                using (var tx = _context.Database.BeginTransaction())
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
                                //On en aura besoin plus tard pour le calcul du prix (ce n'est pas sauvegardé dans la DB).
                                orderClient.OrderLines.ElementAt(i).VegetablesPrice = _context.Menus.First().VegetablesPrice;

                                orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Name = _context.Vegetables.Where(v => v.Id == orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Name;
                                orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Description = _context.Vegetables.Where(v => v.Id == orderClient.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Description;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return new CommWrap<OrderGuid_Com> { RequestStatus = 0 };
                    }
                    tx.Commit();
                }

                //Mettre à jour le prix de la commande avec les nouvelles données.
                orderClient.UpdateTotalAmount();

                string orderTemporaryCode = Guid.NewGuid().ToString();
                ordersToValidate.Add(orderTemporaryCode, communication);

                OrderGuid_Com response = new OrderGuid_Com { Guid_com = orderTemporaryCode, Order_com = communication.Order_com };
                return new CommWrap<OrderGuid_Com> { RequestStatus = 1, Content = response };
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
                    orderDB.AddOrderLine();
                }

                orderDB.AddOrderLine(orderLineAddList);

                _context.SaveChanges();

                return new CommWrap<Order> { RequestStatus = 1, Content = orderDB };
            }
        }*/

        [HttpPost]
        [Route("api/Order/Confirm")]
        public async Task<CommWrap<string>> Confirm([FromBody]CommWrap<string> communication)
        {
            //Si le client indique un echec de son coté (pas assez de crédit par exemple)
            if (communication.RequestStatus == 0)
            {
                if (ordersToValidate.ContainsKey(communication.Content))
                {
                    //Supprimer l'Order de la liste en attente
                    ordersToValidate.Remove(communication.Content);
                    return new CommWrap<string> { RequestStatus = 0 };
                }
            }

            //Si il n'y a pas de eu de soucis chez le client, on continue 
            if (ordersToValidate.ContainsKey(communication.Content))
            {
                OrderCompany_Com orderInValidationInfo = ordersToValidate[communication.Content];

                ////////////
                //Récolte des infos Db nécessaire pour enregistrer la commande.
                ////////////
                Company company = orderInValidationInfo.Company_com;
                Order orderInValidation = orderInValidationInfo.Order_com;

                Company companyDb = await _context.Companies
                                        .Include(emp => emp.Orders.Where(o => o.DateOfDelivery.Equals(orderInValidation.DateOfDelivery)))
                                            .ThenInclude(order => order.OrderLines)
                                                .ThenInclude(odLin => odLin.Sandwich)
                                        .Include(emp => emp.Orders.Where(o => o.DateOfDelivery.Equals(orderInValidation.DateOfDelivery)))
                                            .ThenInclude(order => order.OrderLines)
                                                .ThenInclude(odLin => odLin.OrderLineVegetables)
                                                    .ThenInclude(odLinVeg => odLinVeg.Vegetable)
                                    .SingleOrDefaultAsync(c => c.Id == company.Id && c.Chkcode == company.Chkcode);

                if (companyDb == null)
                {
                    //Si, étrangement, on n'a pas trouvé la company (mesure de sécurité).
                    return new CommWrap<string> { RequestStatus = 0 };
                }
                
                //Si aucune commande pour cette journée n'a été trouvée, on lui assigne la nouvelle commande.
                if (companyDb.Orders.Count == 0)
                {
                    companyDb.Orders.Add(orderInValidation);
                }
                else
                {
                    Order orderDb = company.Orders.First();
                    orderDb.SumUpOrders(orderInValidation);
                }

                orderDB.

                companyDb.Orders.Add()
                }


            }

                //Si commande existe déjà pour cette date, la récupérer
                List<Order> orders = companyDB.Orders.Where(q => q.DateOfDelivery.Equals(delivreryDate)).ToList();

                Order orderDB = null;
                if (orders.Count == 0)
                {
                    orderDB = new Order { DateOfDelivery = delivreryDate, OrderLines = new List<OrderLine>() };

                    companyDB.Orders.Add(orderDB);
                }
                else
                {
                    orderDB = orders.First();
                }

                //Boucler sur chaque OrderLine de l'objet Order reçu par le serveur
                for (int i = 0; i < orderAdd.OrderLines.Count; i++)
                {
                    //Rafraichir les anciennes données avec des nouvelles par mesure de sécurité (les commandes 
                    //aux alentours de 10h sont susceptibles d'être altérées)
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Price = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Price;
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Name = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Name;
                    orderAdd.OrderLines.ElementAt(i).Sandwich.Description = _context.Sandwiches.Where(s => s.Id == orderAdd.OrderLines.ElementAt(i).Sandwich.Id).First().Description;

                    for (int j = 0; j < orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.Count; j++)
                    {
                        orderAdd.OrderLines.ElementAt(i).VegetablesPrice = _context.Menus.First().VegetablesPrice;
                        orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Name = _context.Vegetables.Where(v => v.Id == orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Name;
                        orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Description = _context.Vegetables.Where(v => v.Id == orderAdd.OrderLines.ElementAt(i).OrderLineVegetables.ElementAt(j).Vegetable.Id).First().Description;
                    }

                    //Ajouter chaque OrderLine de l'object Order reçu par le serveur
                    orderDB.AddOrderLine();
                }

                orderDB.AddOrderLine(orderLineAddList);

                _context.SaveChanges();

                return new CommWrap<Order> { RequestStatus = 1, Content = orderDB };
            }
        }
}