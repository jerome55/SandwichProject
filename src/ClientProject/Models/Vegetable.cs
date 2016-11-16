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
    public class Vegetable
    {
        [DataMember]
        public int id { get; set; }
        [Display(Name = "Nom blabla")] // "Qu'est-ce que c'est ?" -Jerome
        [DataMember][NotMapped]
        public string name { get; set; }
        [DataMember][NotMapped]
        public string description { get; set; }
        [DataMember][NotMapped]
        public bool available { get; set; }
    }
}
