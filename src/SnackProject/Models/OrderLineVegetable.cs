using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class OrderLineVegetable
    {
        public OrderLine orderLine { get; set; }
        public int orderLineId { get; set; }

        public Vegetable vegetable { get; set; }
        public int vegetableId { get; set; }
    }
}
