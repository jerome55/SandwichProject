using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Menu
    {
        public int id { get; set; }
        [DataMember]
        public decimal vegetablesPrice { get; set; }

        [DataMember][NotMapped]
        public ICollection<Sandwich> sandwiches { get; set; }
        [DataMember][NotMapped]
        public ICollection<Vegetable> vegetables { get; set; }
        /*
        public Menu(decimal vegetablesPrice, ICollection<Sandwich> sandwiches, ICollection<Vegetable> vegetables)
        {
            this.vegetablesPrice = vegetablesPrice;
            this.sandwiches = sandwiches;
            this.vegetables = vegetables;
        }*/

    }
}
