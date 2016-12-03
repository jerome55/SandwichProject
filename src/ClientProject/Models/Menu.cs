using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class Menu
    {
        [DataMember][NotMapped]
        public decimal VegetablesPrice { get; set; }

        [DataMember][NotMapped]
        public ICollection<Sandwich> Sandwiches { get; set; }
        [DataMember][NotMapped]
        public ICollection<Vegetable> Vegetables { get; set; }

    }
}
