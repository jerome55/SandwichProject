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
        public int Id { get; set; }
        [DataMember]
        public DateTime DateOfDelivery { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public ICollection<OrderLine> OrderLines { get; set; }

        
        /*public Order(DateTime dateOfDelivery, decimal totalAmount, ICollection<OrderLine> orderLines)
        {
            this.dateOfDelivery = dateOfDelivery;
            this.totalAmount = totalAmount;
            this.orderLines = orderLines;
        }*/
    }
}
