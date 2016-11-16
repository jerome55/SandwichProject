using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class OrderLine
    {
        public int id { get; set; }
        public int quantity { get; set; }

        public Sandwich sandwich { get; set; }

        public ICollection<OrderLineVegetable> orderLineVegetables { get; set; }
    }
}
