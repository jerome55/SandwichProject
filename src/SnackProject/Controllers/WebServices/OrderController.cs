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

        ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<CommWrap<Order>> Post([FromBody]OrderCompany_Com communication)
        {
            Order orderAdd = communication.Order_com;
            Company company = communication.Company_com;

            Company companyDB = null;
            List<Company> companyList = _context.Companies.Where(q => q.Id == company.Id && q.Chkcode == company.Chkcode).ToList();

            if(companyList.Count == 0)
            {
                return new CommWrap<Order>{ RequestStatus = 0};
            }
            else
            {
                companyDB = companyList.First();

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


                List<OrderLine> orderLineAddList = orderAdd.OrderLines.ToList();

                orderDB.AddOrderLine(orderLineAddList);

                _context.SaveChanges();

                return new CommWrap<Order> { RequestStatus = 1, Content = orderDB };
            }
        }
    }
}