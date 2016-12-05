using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Date de livraison")]
        [DataMember]
        public DateTime DateOfDelivery { get; set; }
        [Display(Name = "Montant total")]
        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        private ICollection<OrderLine> OrderLines { get; set; }

        public void AddOrderLine(OrderLine ol)
        {
            this.OrderLines.Add(ol);
            this.TotalAmount += ol.GetPrice();
        }

        public void DeleteOrderLine(OrderLine ol)
        {
            this.OrderLines.Remove(ol);
            this.TotalAmount -= ol.GetPrice();
        }
    }
}
