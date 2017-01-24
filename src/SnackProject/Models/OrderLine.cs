using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
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

        //VegetablePrice n'est pas stocké en db et sert juste à calculer le prix dans la méthode GetPrice()
        [NotMapped]
        public decimal VegetablesPrice { get; set; }

        [DataMember]
        public virtual ICollection<OrderLineVegetable> OrderLineVegetables { get; set; } = new List<OrderLineVegetable>();

        public virtual Order Order { get; set; }

        /*public OrderLine(Sandwich sandwich, int quantity, ICollection<OrderLineVegetable> orderLineVegetables)
        {
            this.Sandwich = sandwich;
            this.Quantity = quantity;
            this.OrderLineVegetables = orderLineVegetables;
        }*/

        public bool Equals(OrderLine other)
        {
            if (this.Sandwich.Id != other.Sandwich.Id)
            {
                return false;
            }

            if(this.OrderLineVegetables.Count != other.OrderLineVegetables.Count)
            {
                return false;
            }
            
            for (int i=0; i<this.OrderLineVegetables.Count; ++i)
            {
                bool found = false;
                for(int j=0; j<other.OrderLineVegetables.Count; ++j)
                {
                    if(this.OrderLineVegetables.ElementAt(i).Vegetable.Id == other.OrderLineVegetables.ElementAt(j).Vegetable.Id)
                    {
                        found = true;
                    }
                }
                //Si une des crudités n'a pas été trouvée, pas la peine d'aller plus loin, 
                //c'est un échec.
                if (found == false)
                {
                    return false;
                }
            }

            //Si aucun des tests au dessus n'a échoué ce que forcement les deux OrderLines 
            //sont identiques.
            return true;
        }

        public decimal GetPrice()
        {
            decimal price = this.Sandwich.Price;
            //Si il n'y a pas de crudité, ne pas faire payer le prix du supplément crudité
            if (this.OrderLineVegetables.Count != 0)
            {
                price += this.VegetablesPrice;
            }
            price = price * this.Quantity;

            return price;
        }
    }
}
