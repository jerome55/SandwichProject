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
    public class Menu
    {
        public int Id { get; set; }
        [Display(Name = "Tarif du supplément crudité")]
        [DataMember]
        public decimal VegetablesPrice { get; set; }

        [DataMember]
        [NotMapped]
        public virtual ICollection<Sandwich> Sandwiches { get; set; } = new List<Sandwich>();
        [DataMember]
        [NotMapped]
        public virtual ICollection<Vegetable> Vegetables { get; set; } = new List<Vegetable>();


        /*public Menu(decimal vegetablesPrice, ICollection<Sandwich> sandwiches, ICollection<Vegetable> vegetables)
        {
            this.VegetablesPrice = vegetablesPrice;
            this.Sandwiches = sandwiches;
            this.Vegetables = vegetables;
        }*/
    }
}
