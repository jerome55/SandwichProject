using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class OrderLine
    {
        public int Id { get; set; }
        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public Sandwich Sandwich { get; set; }

        [DataMember]
        public ICollection<OrderLineVegetable> OrderLineVegetables { get; set; }

        
        /*public OrderLine(Sandwich sandwich, int quantity, ICollection<OrderLineVegetable> orderLineVegetables)
        {
            this.sandwich = sandwich;
            this.quantity = quantity;
            this.orderLineVegetables = orderLineVegetables;
        }*/
    }
}
