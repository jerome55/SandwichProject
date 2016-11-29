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
        public int id { get; set; }
        [DataMember]
        public int quantity { get; set; }

        [DataMember]
        public Sandwich sandwich { get; set; }

        [DataMember]
        public ICollection<OrderLineVegetable> orderLineVegetables { get; set; }
    }
}
