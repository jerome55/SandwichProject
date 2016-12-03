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
        public int Id { get; set; }
        [DataMember]
        public decimal VegetablesPrice { get; set; }

        [DataMember][NotMapped]
        public ICollection<Sandwich> Sandwiches { get; set; }
        [DataMember][NotMapped]
        public ICollection<Vegetable> Vegetables { get; set; }


        /*public Menu(decimal vegetablesPrice, ICollection<Sandwich> sandwiches, ICollection<Vegetable> vegetables)
        {
            this.vegetablesPrice = vegetablesPrice;
            this.sandwiches = sandwiches;
            this.vegetables = vegetables;
        }*/
    }
}
