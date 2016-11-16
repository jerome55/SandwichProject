using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string dateOfDelivrery { get; set; }
        public double totalPrice { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }
    }
}
