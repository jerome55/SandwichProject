using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Sandwich {
        [DataMember]
        public int Id { get; set; }
        [Display(Name = "Nom"),Required]
        [DataMember]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [DataMember]
        public string Description { get; set; }
        [Display(Name = "Prix"),Range(0.0,100.0)]
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        [Display(Name = "Disponible")]
        public bool Available { get; set; }
    }
}
