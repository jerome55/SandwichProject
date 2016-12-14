using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class OrderLine
    {
        public int Id { get; set; }
        [Display(Name = "Quantité")]
        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public Sandwich Sandwich { get; set; }

        [DataMember]
        public ICollection<OrderLineVegetable> OrderLineVegetables { get; set; }

        
        /*public OrderLine(Sandwich sandwich, int quantity, ICollection<OrderLineVegetable> orderLineVegetables)
        {
            this.Sandwich = sandwich;
            this.Quantity = quantity;
            this.OrderLineVegetables = orderLineVegetables;
        }*/

        public bool Equals(OrderLine other)
        {
            List<OrderLineVegetable> listOrderLineVegetables = OrderLineVegetables.ToList();
            List<OrderLineVegetable> listOther = other.OrderLineVegetables.ToList();

            bool follow = Sandwich.Id==other.Sandwich.Id && Sandwich.Available && other.Sandwich.Available;

            for (int i=0;i< listOther.Count && follow==true; ++i)
            {
                for(int j=0;j< listOrderLineVegetables.Count && follow==true; ++i)
                {
                    follow = listOther[i].Equals(listOrderLineVegetables[j]);
                }
            }


            return follow;
        }

        public decimal GetPrice()
        {
            return Sandwich.Price * Quantity + (new Menu()).VegetablesPrice;
        }
    }
}
