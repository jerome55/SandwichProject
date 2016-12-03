using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Vegetable
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [Display(Name = "Nom")]
        public string name { get; set; }
        [DataMember]
        [Display(Name = "Description")]
        public string description { get; set; }
        [DataMember]
        [Display(Name = "Disponible")]
        public bool available { get; set; }

        /*
        public Vegetable(string name, string description, bool available)
        {
            this.name = name;
            this.description = description;
            this.available = available;
        }*/
    }
}
