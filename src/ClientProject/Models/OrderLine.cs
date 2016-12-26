using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "Sandwich")]
        [DataMember]
        public virtual Sandwich Sandwich { get; set; }

        //VegetablePrice n'est pas stocké en db et sert juste à calculer le prix dans la méthode GetPrice()
        [NotMapped]
        public decimal VegetablesPrice { get; set; }

        [Display(Name = "Liste des crudités")]
        [DataMember]
        public virtual ICollection<OrderLineVegetable> OrderLineVegetables { get; set; } = new List<OrderLineVegetable>();

        public virtual Order Order { get; set; }


        public decimal GetPrice()
        {
            decimal price = this.Sandwich.Price;
            if (this.OrderLineVegetables.Count != 0)
            {
                price += this.VegetablesPrice;
            }
            return price;
        }

        public void AddVegetable(Vegetable veg)
        {
            OrderLineVegetable olVeg = new OrderLineVegetable() { OrderLine = this, Vegetable = veg };
            this.OrderLineVegetables.Add(olVeg);
        }
    }
}
