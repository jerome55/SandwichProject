﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class Vegetable
    {
        [DataMember]
        public int Id { get; set; }
        //[Display(Name = "Nom blabla")] // "Qu'est-ce que c'est ?" -Jerome
        [DataMember][NotMapped]
        public string Name { get; set; }
        [DataMember][NotMapped]
        public string Description { get; set; }
        [DataMember][NotMapped]
        public bool Available { get; set; }
    }
}
