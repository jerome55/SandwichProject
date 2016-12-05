using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class OrderLineVegetable
    {
        public OrderLine OrderLine { get; set; }
        public int OrderLineId { get; set; }

        [DataMember]
        public Vegetable Vegetable { get; set; }
        public int VegetableId { get; set; }

        
        /*public OrderLineVegetable(OrderLine orderLine, Vegetable vegetable)
        {
            this.OrderLine = orderLine;
            this.Vegetable = vegetable;
        }*/
    }
}
