using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class OrderLine
    {
        public int Id { get; set; }
        [Display(Name = "Quantité")]
        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public Sandwich Sandwich { get; set; }

        [DataMember]
        public ICollection<OrderLineVegetable> OrderLineVegetables { get; set; }


        public decimal GetPrice()
        {
            return Sandwich.Price + (new Menu()).VegetablesPrice;
        }
    }
}
