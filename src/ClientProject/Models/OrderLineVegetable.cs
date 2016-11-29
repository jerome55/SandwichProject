using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class OrderLineVegetable
    {
        public OrderLine orderLine { get; set; }
        public int orderLineId { get; set; }

        [DataMember]
        public Vegetable vegetable { get; set; }
        public int vegetableId { get; set; }
    }
}
