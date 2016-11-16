using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class Menu
    {
        public int id { get; set; }
        public double vegetablesPrice { get; set; }

        [NotMapped]
        public ICollection<Sandwich> sandwiches { get; set; }
        [NotMapped]
        public ICollection<Vegetable> vegetables { get; set; }
        
    }
}
