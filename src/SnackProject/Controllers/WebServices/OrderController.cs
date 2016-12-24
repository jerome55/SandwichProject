using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            if (companyList.Count == 0)
            {
                return new CommWrap<Order> { RequestStatus = 0 };
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
                if (orders.Count == 0)
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

        [HttpDelete]
        public async Task<int> delete([FromBody]OrderCompany_Com communication)
        {
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

            if(delivreryDate.Equals(communication.Order_com.DateOfDelivery))
            {
                Company company = _context.Companies
                    .Include(compQ => compQ.Orders.Where(q=>q.DateOfDelivery.Equals(delivreryDate)).First())
                        .ThenInclude(orderQ => orderQ.OrderLines)
                        .ThenInclude(orderLineQ => orderLineQ.OrderLineVegetables)
                        .ThenInclude(veg => veg.Vegetable)
                    .Include(compQ => compQ.Orders.Where(q => q.DateOfDelivery.Equals(delivreryDate)).First())
                        .ThenInclude(orderQ => orderQ.OrderLines)
                        .ThenInclude(orderLineQ => orderLineQ.Sandwich)
                    .Where(q => q.Id == communication.Company_com.Id).First();

                Order order = company.Orders.First();

                for(int i=0;i<order.OrderLines.Count;++i)
                {
                    int j;
                    for (j = 0; j < communication.Order_com.OrderLines.Count && !order.OrderLines.ElementAt(i).Equals(communication.Order_com.OrderLines.ElementAt(j)); ++j) ;

                    order.TotalAmount -= communication.Order_com.OrderLines.ElementAt(j).Quantity * communication.Order_com.OrderLines.ElementAt(j).GetPrice();


                    order.OrderLines.ElementAt(i).Quantity -= communication.Order_com.OrderLines.ElementAt(j).Quantity;

                    if (order.OrderLines.ElementAt(i).Quantity == 0)
                    {
                        _context.OrderLines.Attach(order.OrderLines.ElementAt(i));
                        _context.OrderLines.Remove(order.OrderLines.ElementAt(i));
                    }
                    else
                    {
                        _context.OrderLines.Attach(order.OrderLines.ElementAt(i));
                        var entry = _context.Entry(order.OrderLines.ElementAt(i));
                        entry.Property(e => e.Quantity).IsModified = true;
                    }

                }

                _context.Orders.Attach(order);
                var entryOrder = _context.Entry(order);
                entryOrder.Property(e => e.TotalAmount).IsModified = true;

                _context.SaveChanges();

                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}