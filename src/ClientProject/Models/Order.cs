using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class Order
    {
        public int id { get; set; }

        [DataMember]
        public DateTime dateOfDelivery { get; set; }
        [DataMember]
        public decimal totalAmount { get; set; }
        [DataMember]
        private ICollection<OrderLine> orderLines { get; set; }

        public void addOrderLine(OrderLine ol)
        {
            this.orderLines.Add(ol);
            this.totalAmount += ol.getPrice();
        }

        public void deleteOrderLine(OrderLine ol)
        {
            this.orderLines.Remove(ol);
            this.totalAmount -= ol.getPrice();
        }
    }
}
