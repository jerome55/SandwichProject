﻿using System;
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
        [Display(Name = "Name")]
        [DataMember]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [DataMember]
        public string Description { get; set; }
        [Display(Name = "Prix")]
        [DataMember]
        public decimal Price { get; set; }
        [Display(Name = "Disponibilité")]
        public bool Available { get; set; }

        
        /*public Sandwich(string name, string description, decimal price, bool available)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Available = available;
        }*/
    }
}
