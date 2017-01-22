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
        public int Id { get; set; }
        [Display(Name = "Nom")]
        [DataMember]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [DataMember]
        public string Description { get; set; }
        [Display(Name = "Disponibilité")]
        public bool Available { get; set; }


        /*public Vegetable(string name, string description, bool available)
        {
            this.Name = name;
            this.Description = description;
            this.Available = available;
        }*/
    }
}
