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
    public class Menu
    {
        [Display(Name = "Tarif du supplément crudité")]
        [DataMember][NotMapped]
        public decimal VegetablesPrice { get; set; }

        [DataMember][NotMapped]
        public virtual ICollection<Sandwich> Sandwiches { get; set; } = new List<Sandwich>();
        [DataMember][NotMapped]
        public virtual ICollection<Vegetable> Vegetables { get; set; } = new List<Vegetable>();

    }
}
