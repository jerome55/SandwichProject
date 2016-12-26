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
        public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public virtual Company Company { get; set; }

        /*public Order(DateTime dateOfDelivery, decimal totalAmount, ICollection<OrderLine> orderLines)
        {
            this.DateOfDelivery = dateOfDelivery;
            this.TotalAmount = totalAmount;
            this.OrderLines = orderLines;
        }*/

        public void UpdateTotalAmount()
        {
            this.TotalAmount = 0;
            for (int i=0; i<this.OrderLines.Count; i++)
            {
                this.TotalAmount += this.OrderLines.ElementAt(i).GetPrice();
            }
        }

        public void SumUpOrders(Order otherOrder)
        {
            //Combiner les OrderLines (si identique increment quantité, sinon add to list)
            for (int i=0; i<otherOrder.OrderLines.Count; ++i)
            {
                bool found = false;
                int j = 0;
                for (j = 0; j < this.OrderLines.Count && found == false; ++j)
                {
                    if (otherOrder.OrderLines.ElementAt(i).Equals(this.OrderLines.ElementAt(j)))
                    {
                        found = true;
                    }
                }

                OrderLine current = otherOrder.OrderLines.ElementAt(i);
                if (found == false)
                {
                    this.OrderLines.Add(current);
                }
                else
                {
                    this.OrderLines.ElementAt(j).Quantity += current.Quantity;
                }
            }
            //Additionner les totaux
            this.TotalAmount += otherOrder.TotalAmount;
        }
    }
}
