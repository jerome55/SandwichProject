using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
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
        public ICollection<OrderLine> OrderLines { get; set; }

        
        /*public Order(DateTime dateOfDelivery, decimal totalAmount, ICollection<OrderLine> orderLines)
        {
            this.DateOfDelivery = dateOfDelivery;
            this.TotalAmount = totalAmount;
            this.OrderLines = orderLines;
        }*/
    }
}
