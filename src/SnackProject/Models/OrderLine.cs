using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class OrderLine
    {
        public int ID { get; set; }
        public int quantity { get; set; }

        public Sandwich Sandwichs { get; set; }
    }
}
