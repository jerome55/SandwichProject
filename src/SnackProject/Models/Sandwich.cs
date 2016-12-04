using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Sandwich
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        public bool Available { get; set; }

        
        /*public Sandwich(string name, string description, decimal price, bool available)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.available = available;
        }*/
    }
}
