using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
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
        public ICollection<OrderLine> orderLines { get; set; }
        /*
        public Order(DateTime dateOfDelivery, decimal totalAmount, ICollection<OrderLine> orderLines)
        {
            this.dateOfDelivery = dateOfDelivery;
            this.totalAmount = totalAmount;
            this.orderLines = orderLines;
        }*/
    }
}
