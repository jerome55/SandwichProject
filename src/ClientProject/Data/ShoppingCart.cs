using System;
using System.Linq;
using System.Threading.Tasks;
using ClientProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ClientProject.Data
{
    public class ShoppingCart
    {
        public static Order GetCartContent(ClaimsPrincipal User, HttpContext HttpContext)
        {
            //On détermine si la commande est pour aujourd'hui ou pour demain (après 10h).
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

            //On récupère l'objet order stocké dans la session de l'utilisateur (son panier).
            string serializable = HttpContext.Session.GetString("cart");

            Order cartOrder;
            if (serializable == null || serializable.Equals(""))
            {
                cartOrder = new Order { DateOfDelivery = delivreryDate, TotalAmount = 0, OrderLines = new List<OrderLine>() };
            }
            else
            {
                cartOrder = JsonConvert.DeserializeObject<Order>(serializable);
            }
            //Si pour des raison spécial l'objet est bien dans la session mais à null (invalidation), on le recrée.
            if (cartOrder == null)
            {
                cartOrder = new Order { DateOfDelivery = delivreryDate, TotalAmount = 0, OrderLines = new List<OrderLine>() };
            }

            return cartOrder;
        }

        public static void UpdateCartContent(HttpContext HttpContext, Order newCartOrder)
        {
            string serializable = JsonConvert.SerializeObject(newCartOrder);
            HttpContext.Session.SetString("cart", serializable);
        }
    }
}
