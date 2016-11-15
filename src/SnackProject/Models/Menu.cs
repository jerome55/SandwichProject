using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class Menu
    {
        public int ID { get; set; }
        public double crudityPrice { get; set; }

        public ICollection<Crudity> Crudities { get; set; }
        public ICollection<Sandwich> Sandwichs { get; set; }
    }
}
