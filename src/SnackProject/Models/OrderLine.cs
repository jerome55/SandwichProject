﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class OrderLine
    {
        public int id { get; set; }

        [DataMember]
        public int quantity {
            get { return quantity; }
            set{
                if(value > 0)
                {
                    quantity = value;
                }
                else
                {
                    throw new Exception("Quantité inférieure a 0");
                }
            }
        }

        [DataMember]
        public Sandwich sandwich { get; set; }

        [DataMember]
        public ICollection<OrderLineVegetable> orderLineVegetables { get; set; }

        public OrderLine(Sandwich sandwich, int quantity, ICollection<OrderLineVegetable> orderLineVegetables)
        {
            this.sandwich = sandwich;
            this.quantity = quantity;
            this.orderLineVegetables = orderLineVegetables;
        }
    }
}
