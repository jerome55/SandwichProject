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
        public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public virtual Employee Employee { get; set; }


        public void AddOrderLine(OrderLine ol)
        {
            this.OrderLines.Add(ol);
            ol.Order = this;
            this.TotalAmount += ol.GetPrice();
        }

        public void DeleteOrderLine(int id)
        {
            DeleteOrderLine(OrderLines.ElementAt(id));
        }

        public void DeleteOrderLine(OrderLine ol)
        {
            this.OrderLines.Remove(ol);
            ol.Order = null;
            this.TotalAmount -= ol.GetPrice();
        }

        public void SumUpOrders(Order otherOrder)
        {
            //Combiner les OrderLines (si identique increment quantité, sinon add to list)
            for (int i = 0; i < otherOrder.OrderLines.Count; ++i)
            {
                bool found = false;
                int j = 0;
                while (j < this.OrderLines.Count && found == false)
                {
                    if (otherOrder.OrderLines.ElementAt(i).Equals(this.OrderLines.ElementAt(j)))
                    {
                        found = true;
                    }
                    else
                    {
                        ++j;
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
